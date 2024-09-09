export interface PoslednjePozicijaVozilaOdgovor {
    id: number;
    tablica: string;
    tip: string;
    poslednjaPozicija: {
        id: string;
        koordinate: number[];
        brzinaKretanja: number;
        datumUnosa: Date | string;
    };

    grafikonId: string;
    podaci: { x: string, y: number }[];
    grafikon: any;
}
