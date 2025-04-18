# PC-Rodikliai Projekto Planas

## v0.1.0 - Bazinė funkcionalumo versija
- [x] Sukurti bazinę projekto struktūrą
- [x] Implementuoti MainViewModel
- [x] Implementuoti Metric modelį
- [x] Sukurti pagrindinį langą su metrikų atvaizdavimu
- [x] Implementuoti CPU apkrovos matavimą
- [x] Implementuoti RAM naudojimo matavimą
- [x] Implementuoti disko naudojimo matavimą
- [x] Implementuoti tinklo greičio matavimą

## v0.2.0 - Realaus laiko atnaujinimai
- [x] Pridėti realaus laiko metrikų atnaujinimą
- [x] Implementuoti grafikus metrikų vizualizacijai
- [ ] Pridėti istorijos kaupimą
- [ ] Pridėti galimybę eksportuoti duomenis

## v0.3.0 - Vartotojo sąsajos patobulinimai
- [ ] Pridėti tamsią/šviesią temą
- [ ] Pridėti galimybę keisti atnaujinimo intervalą
- [ ] Pridėti galimybę pasirinkti rodomas metrikas
- [ ] Pridėti įspėjimus apie kritines reikšmes

## v0.4.0 - Papildomos funkcijos
- [ ] Pridėti temperatūros matavimus
- [ ] Pridėti procesų sąrašą
- [ ] Pridėti galimybę nustatyti įspėjimų ribas
- [ ] Pridėti sistemos informacijos langą

## v1.0.0 - Pirmasis oficialus leidimas
- [ ] Optimizuoti veikimą
- [ ] Ištaisyti žinomus klaidas
- [ ] Pridėti dokumentaciją
- [ ] Paruošti diegimo paketą

## Ateities planai
- Pridėti palaikymą kitiems OS
- Pridėti debesies integraciją
- Pridėti mobilią aplikaciją
- Pridėti API

## Techninė specifikacija

### Technologijos
- WPF (.NET 8)
- LibreHardwareMonitor
- LiveCharts2 (grafikams)
- Hardcodet.NotifyIcon.Wpf (sistemos dėklo ikonai)

### Architektūra
- MVVM pattern
- Dependency Injection
- Repository pattern monitoringo duomenims
- Observer pattern realaus laiko atnaujinimams

### Failų struktūra
```
PC-Rodikliai/
├── src/
│   ├── PC-Rodikliai/
│   │   ├── App.xaml
│   │   ├── MainWindow.xaml
│   │   ├── Components/
│   │   │   ├── Clock/
│   │   │   ├── Graphs/
│   │   │   ├── Metrics/
│   │   │   └── Settings/
│   │   ├── Models/
│   │   ├── ViewModels/
│   │   ├── Services/
│   │   │   ├── HardwareMonitor/
│   │   │   ├── AlertService/
│   │   │   └── HotkeyService/
│   │   └── Themes/
│   └── PC-Rodikliai.Tests/
├── docs/
└── tools/
``` 