import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { PoslednjePozicijaVozilaOdgovor } from "../modeli/poslednjePoyicijeVoyila.odgovor.model";

@Injectable({
    providedIn: 'root'
})

export class VozilaServis {
    private apiAdresa = 'https://localhost:44322/api/vozila';

    constructor(
        private http: HttpClient
    ) { }

    vratiPoslednjePozicijeVozila() {
        return this.http.get<PoslednjePozicijaVozilaOdgovor[]>(
            `${this.apiAdresa}/poslednjePozicije`);
    }
}
