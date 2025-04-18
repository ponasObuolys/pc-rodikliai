# PC-Rodikliai Projekto Planas

## Dabartinė Versija (v0.2.0)

### ✅ Įgyvendinta

1. Bazinė funkcionalumas:
   - CPU metrikos stebėjimas
   - RAM naudojimo stebėjimas
   - GPU metrikos
   - Tinklo greičio stebėjimas
   - Diskų naudojimo stebėjimas

2. Vartotojo sąsaja:
   - Metrikų kortelės
   - Realaus laiko grafikai
   - Taskbar integracija

3. Testavimas:
   - ✅ MetricsViewModel testai
   - ✅ HardwareMonitorService ribinių atvejų testai
   - ✅ UI komponentų testai (Charts)
   - ✅ Integraciniai testai
   - ✅ Našumo testai

### 🔄 Vykdoma

1. Vartotojo sąsajos tobulinimas:
   - Tamsios/šviesios temos palaikymas
   - Metrikų kortelių pertvarkymas
   - Grafikų stilių tobulinimas

2. Papildomos funkcijos:
   - Įspėjimai apie kritines metrikas
   - Metrikų istorijos išsaugojimas
   - Eksportavimas į CSV

### 📅 Planuojama

1. v0.3.0:
   - Nustatymų langas
   - Metrikų atnaujinimo dažnio konfigūracija
   - Grafikų laikotarpio pasirinkimas
   - Papildomi grafikai ir statistika

2. v0.4.0:
   - Procesų stebėjimas
   - Detalesnė CPU informacija
   - Detalesnė GPU informacija
   - Temperatūrų stebėjimas

3. v1.0.0:
   - Stabilumo patobulinimai
   - Našumo optimizacijos
   - Dokumentacijos atnaujinimas
   - Instaliacijos vedlys

## Testavimo Planas

### Unit Testai

1. ✅ MetricsViewModel:
   - Duomenų atnaujinimo testai
   - Grafikų atnaujinimo testai
   - Formatavimo funkcijų testai

2. ✅ HardwareMonitorService:
   - Tinklo greitaveikos ribiniai atvejai
   - Disko užimtumo ribiniai atvejai
   - Klaidų apdorojimo scenarijai

3. ✅ UI Komponentai:
   - Grafikų atvaizdavimo testai
   - Laikrodžio komponento testai
   - Taskbar ikonos testai

### Integraciniai Testai

✅ Įgyvendinta:
- Pilnas duomenų kelias nuo serviso iki UI
- Vartotojo sąveikos scenarijai
- Sistemos stabilumo testai

### Našumo Testai

✅ Įgyvendinta:
- Atminties naudojimo testai
- CPU apkrovos testai
- UI atsako laiko testai

## Žinomi Trūkumai

1. 🐛 UI Problemos:
   - Grafikų atnaujinimo vėlavimas esant didelei apkrovai
   - Netikslus atminties naudojimo atvaizdavimas kai kuriose sistemose

2. 🔧 Techninės Problemos:
   - Didelė RAM naudojimas ilgai veikiant
   - Ne visų GPU modelių palaikymas
   - Neoptimalus tinklo greičio matavimas

## Prioritetai

1. 🔴 Aukštas:
   - Atminties nutekėjimo problemos sprendimas
   - GPU palaikymo išplėtimas
   - Našumo optimizacija

2. 🟡 Vidutinis:
   - Nustatymų lango implementacija
   - Grafikų tobulinimas
   - Dokumentacijos atnaujinimas

3. 🟢 Žemas:
   - Papildomos statistikos
   - UI temos
   - Lokalizacijos

## Sekantys Žingsniai

1. Stabilumo gerinimas:
   - Atminties nutekėjimo taisymas
   - Išimčių apdorojimo tobulinimas
   - Testavimo aprėpties didinimas

2. Vartotojo sąsajos tobulinimas:
   - Nustatymų lango kūrimas
   - Grafikų interaktyvumo didinimas
   - Vartotojo patirties gerinimas

3. Dokumentacija:
   - API dokumentacijos atnaujinimas
   - Vartotojo vadovo sukūrimas
   - Diegimo instrukcijų paruošimas

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
  - [x] CPU apkrovos grafikas
  - [x] RAM naudojimo grafikas
  - [x] Disko naudojimo grafikas
  - [x] Tinklo greičio grafikas
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