# PC-Rodikliai Projekto Planas

## v0.1.0 - Bazinė funkcionalumo versija ✅
- [x] Sukurti bazinę projekto struktūrą
- [x] Implementuoti MainViewModel
- [x] Implementuoti Metric modelį
- [x] Sukurti pagrindinį langą su metrikų atvaizdavimu
- [x] Implementuoti CPU apkrovos matavimą
- [x] Implementuoti RAM naudojimo matavimą
- [x] Implementuoti disko naudojimo matavimą
- [x] Implementuoti tinklo greičio matavimą

## v0.2.0 - Realaus laiko atnaujinimai (Dabartinis prioritetas)
- [x] Pridėti realaus laiko metrikų atnaujinimą
- [ ] Implementuoti grafikus metrikų vizualizacijai (LiveCharts2)
  - [ ] CPU apkrovos grafikas
  - [ ] RAM naudojimo grafikas
  - [ ] Disko naudojimo grafikas
  - [ ] Tinklo greičio grafikas
- [ ] Pridėti istorijos kaupimą
  - [ ] Duomenų bazės struktūros sukūrimas
  - [ ] Duomenų išsaugojimo logika
  - [ ] Istorinių duomenų atvaizdavimas grafikuose
- [ ] Pridėti galimybę eksportuoti duomenis

## v0.3.0 - Nustatymų langas ir įspėjimai
- [x] Pridėti sistemos dėklo ikoną
- [x] Pridėti administratoriaus teisių reikalavimą
- [ ] Sukurti nustatymų langą
  - [ ] Tamsios/šviesios temos pasirinkimas
  - [ ] Atnaujinimo intervalo keitimas
  - [ ] Rodomų metrikų pasirinkimas
- [ ] Implementuoti įspėjimų sistemą
  - [ ] Įspėjimų ribų nustatymas
  - [ ] Pranešimų rodymas
  - [ ] Garsiniai signalai (pasirinktinai)

## v0.4.0 - Papildomos funkcijos
- [ ] Pridėti temperatūros matavimus
- [ ] Pridėti procesų sąrašą
- [ ] Pridėti sistemos informacijos langą
- [ ] Pridėti hotkey palaikymą

## v1.0.0 - Pirmasis oficialus leidimas
- [ ] Optimizuoti veikimą
- [ ] Ištaisyti žinomas klaidas
- [ ] Pridėti dokumentaciją
- [ ] Paruošti diegimo paketą
- [ ] Atlikti testavimą
- [ ] Parengti vartotojo vadovą

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
- SQLite (duomenų saugojimui)

### Architektūra
- MVVM pattern
- Dependency Injection
- Repository pattern monitoringo duomenims
- Observer pattern realaus laiko atnaujinimams
- Command pattern vartotojo veiksmams

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