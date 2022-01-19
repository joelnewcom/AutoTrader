
## AssetPairs to trade

* BCHCHF        BCH/CHF
* BTCCHF        BTC/CHF
* EOScoinCHF    EOS/CHF
* GBPCHF        GBP/CHF
* HCPCHF        TREE/CHF
* LINKCHF       LINK/CHF
* LKKCHF        LKK/CHF
* LKK1YCHF      LKK1Y/CHF
* LKK2YCHF      LKK2Y/CHF
* LTCCHF        LTC/CHF
* USDCHF        USD/CHF
* XLMCHF        XLM/CHF
* XRPCHF        XRP/CHF
* EURCHF        EUR/CHF
* ETHCHF        ETH/CHF

## Run locally production build
1. Open cmd in ./AutoTrader and run following two commands (The same as defined in master_lykke-trader-app.yml github workflow)
2. run dotnet build --configuration Release
3. run: dotnet publish -c Release -o <whatever>/myapp
4. Goto <whatever>/myapp and run AutoTrader.exe

### Database

## Entity framework
Following commands are used to build up the datebase schema for entity framework:

```
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
* added new entity logBook
dotnet ef migrations add AddLogBookTable
dotnet ef database update
* changed AssetPair entity
dotnet ef migrations add NewFieldsInAssetPair
dotnet ef database update
* Added primary key for exceptionLog entries
dotnet ef migrations add ExceptionLogWithoutKeyLess
dotnet ef database update
* Previous migration was wrong so we undo it by:
dotnet ef database update NewFieldsInAssetPair
dotnet ef migrations remove
dotnet ef migrations add NewExceptionLogEntity
dotnet ef database update
```

## Download prod db 
1. Goto app service in azure portal.
2. Click on Advanced Tool (Under development tools) -> This leads to KuDo
3. Goto to Debug Console (Choose CMD or Powershell. We only want to use filebrowser)
4. Navigate ```./site/wwwroot/```
5. Download ```AutoTrader.db```

## Put money into API Wallet
Put money into your trading wallet
Goto https://wallet.lykke.com/wallets/hft
Select the API wallet you want to deposit money
Click on "TRANSER"
Fill out the form and use Microsoft Authenticator app for MFA code
Use the Lykke app to verify the QR code. If popup doesn't appear on the app, restart the app