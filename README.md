# Proje Kurulum Rehberi

Bu proje, **SQL Server 2022** veritabanı üzerinde çalışmaktadır. Veritabanını hızlıca başlatmak için Docker kullanılmaktadır.

---

## 🚀 Gereksinimler
- [Docker](https://www.docker.com/get-started) yüklü olmalıdır.
- [.NET SDK](https://dotnet.microsoft.com/en-us/download) (projenin hedef versiyonuna uygun) yüklü olmalıdır.
- En az **2 GB RAM** Docker konteynerine ayrılmış olmalıdır.
- Boşta **1433** portu bulunmalıdır.

---

## 📦 Veritabanını Başlatma

Aşağıdaki komut ile SQL Server 2022 konteynerini başlatabilirsiniz:

```bash
docker run \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=Msg.18273645" \
  -p 1433:1433 \
  --name sqlserver2022 \
  -d mcr.microsoft.com/mssql/server:2022-latest
