# SQL Schema Diff

MSSQL için schema diff aracı. İki SSMS şema betiğini karşılaştırır:

- **Kaynak** = olması gereken şema (örn. yeni sürümün şeması)
- **Hedef**  = güncellenecek veritabanının şeması

Farkları listeler, her farkı yan yana **şema diff** olarak gösterir ve seçtiğin farklar için
hedefi kaynakla eşitleyen, **tekrar çalıştırılabilir** bir güncelleme betiği üretir.

## Çalıştırma
1. Visual Studio 2022 ile `SqlSchemaDiff.csproj` dosyasını aç (.NET 8 SDK gerekir).
2. NebulaUI paketi yerel feed'den gelir (`nuget.config` → `C:\Users\PC\source\LocalPackages`).
3. F5 ile çalıştır.
4. Kaynak ve hedef betiği seç (veya pencereye sürükle) → Karşılaştır (F5) →
   farkları incele / işaretle → Script oluştur (Ctrl+G) → Kaydet (Ctrl+S).

## Neleri karşılaştırır
Tablolar, kolonlar (yeni / tanımı farklı), foreign key'ler ve view'ler.
Sadece hedefte olan kolonlar bilgi olarak gösterilir, scriptlenmez.
"Kolon tanımı farklı" satırları riskli olduğu için varsayılan olarak seçili gelmez.

## Kod yapısı
- Models/                            : saf veri sınıfları
- Services/SqlSchemaParser.cs        : betiği okuyup tablo/kolon/view/FK çıkarır
- Services/SchemaComparer.cs         : kaynak ve hedef şemayı karşılaştırır
- Services/LineDiff.cs               : yan yana diff için satır bazlı LCS
- Services/SqlScriptTemplates.cs     : üretilen T-SQL parçaları
- Services/UpgradeScriptBuilder.cs   : seçilenleri tek dosyada birleştirir
- Views/SchemaDiffView.cs            : yan yana şema diff kontrolü
- Helpers/                           : SQL renklendirme, fark türü renkleri
- MainForm.cs / MainForm.Designer.cs : ekran (iş mantığı / Designer)
