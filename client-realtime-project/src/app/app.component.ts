import { AfterViewInit, Component, OnDestroy, OnInit, ChangeDetectorRef } from '@angular/core';
import { PoslednjePozicijaVozilaOdgovor } from './modeli/poslednjePoyicijeVoyila.odgovor.model';
import { VozilaServis } from './servisi/vozila.servis';

import * as signalr from '@microsoft/signalr';
import * as ApexCharts from 'apexcharts';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy, AfterViewInit {

  private habKonekcija!: signalr.HubConnection;
  
  vozila: PoslednjePozicijaVozilaOdgovor[] = [];
  opcijeGrafikona = {
    chart: {
      type: "line",
      height: 250,
      animations: {
        enabled: true,
        easing: "linear",
        dynamicAnimation: {
          speed: 1000,
        },
      },
      toolbar: {
        show: false,
      },
    },
    xaxis: {
      labels: {
        formatter: (value: any): string => {
          return value;
          // return moment(value).format("dd.MM.yy hh:mm:ss");
        },
        show: false,
      },
    }
  };

  constructor(
    private vozilaServis: VozilaServis,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.vratiPoslednjePozicijeVozila();
    this.startujKonekcijuISlusajPromene();
  }
  
  ngAfterViewInit(): void {
    this.inicijalizujGrafikone();
  }
  
  private vratiPoslednjePozicijeVozila(): void {
    this.vozilaServis.vratiPoslednjePozicijeVozila().subscribe(
      odgovor => this.vozila = odgovor,
      error => console.log(error),
      () => this.inicijalizujGrafikone());
  }


  private inicijalizujGrafikone(): void {
    this.vozila.forEach(vozilo => {
      vozilo.podaci = [{
        x: vozilo.poslednjaPozicija.datumUnosa.toString(),
        y: vozilo.poslednjaPozicija.brzinaKretanja,
      }];
      const elementGrafikona = document.querySelector(`#${vozilo.grafikonId}`);
      if (elementGrafikona) {
        vozilo.grafikon = new ApexCharts(elementGrafikona, this.opcijeGrafikona);
        vozilo.grafikon.render();
      }
    });
  }

  private startujKonekcijuISlusajPromene(): void {
    this.habKonekcija = new signalr.HubConnectionBuilder()
      .withUrl('https://localhost:44322/pozicijeVozilaHab', { 
        withCredentials: true 
      }).build();
    this.habKonekcija
      .start()
      .then(() => this.slusajPoslednjePozicije())
      .catch(greska => console.error('Serverska greska: ' + greska));
  }

  private slusajPoslednjePozicije() {
    this.habKonekcija.on('mojametoda', (poslednjaPozicija: any) => {
      this.mapirajPoslednjuPozicijuVozila(poslednjaPozicija);
    });
  }

  private mapirajPoslednjuPozicijuVozila(poslednjaPozicija: any): void {
    const model = {
      brzinaKretanja: poslednjaPozicija.brzina,
      datumUnosa: new Date(poslednjaPozicija.datumUnosaFormat),
      id: poslednjaPozicija.id,
      koordinate: poslednjaPozicija.koordinate,
    };

    const index = this.vozila.findIndex(
      vozilo => vozilo.id === poslednjaPozicija.voziloId);

    if (index !== -1) {
      this.vozila[index].poslednjaPozicija = model;
      this.vozila[index].podaci = [
        ...this.vozila[index].podaci,
        { x: model.datumUnosa.toString(), y: model.brzinaKretanja },
      ];
      this.cdr.detectChanges();
    }
  }

  ngOnDestroy() {
    if (this.habKonekcija) {
      this.habKonekcija.stop().then(
        () => console.log('SignalR connection stopped'));
    }
  }
}
