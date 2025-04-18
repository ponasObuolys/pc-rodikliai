# PC-Rodikliai Projekto Planas

## v0.1.0 - Bazinė struktūra
- [ ] Projekto inicializacija su WPF
- [ ] LibreHardwareMonitor integravimas
- [ ] Bazinė šoninės juostos UI struktūra
- [ ] Laikrodžio komponentas
- [ ] Bazinis stilius ir temos

## v0.2.0 - Pagrindiniai rodikliai
- [ ] CPU monitoringas
  - [ ] Apkrova
  - [ ] Temperatūra
  - [ ] Dažnis
- [ ] RAM monitoringas
  - [ ] Naudojimas
  - [ ] Laisva atmintis
- [ ] GPU monitoringas
  - [ ] Apkrova
  - [ ] Temperatūra
  - [ ] VRAM naudojimas
- [ ] Tinklo monitoringas
  - [ ] Atsisiuntimas
  - [ ] Įkėlimas
- [ ] Diskų monitoringas
  - [ ] Naudojimas
  - [ ] Laisva vieta

## v0.3.0 - Grafikai
- [ ] Universalus grafikų komponentas
- [ ] Realaus laiko duomenų atnaujinimas
- [ ] Grafikų stilizavimas
- [ ] Grafikų istorijos išsaugojimas

## v0.4.0 - Pritaikymas
- [ ] Temos keitimas
- [ ] Spalvų schemos keitimas
- [ ] Rodiklių rodymo/slėpimo nustatymai
- [ ] DPI palaikymas
- [ ] Pozicijos ir dydžio keitimas

## v0.5.0 - Įspėjimai ir spartieji klavišai
- [ ] Įspėjimų sistema
- [ ] Įspėjimų konfigūravimas
- [ ] Sparčiųjų klavišų sistema
- [ ] Sparčiųjų klavišų konfigūravimas

## v1.0.0 - Išleidimas
- [ ] Kodo optimizavimas
- [ ] Testavimas
- [ ] Dokumentacija
- [ ] Diegimo programa

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