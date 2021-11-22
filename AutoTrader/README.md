# lykkeTrader

* [Public API Definition](https://public-api.lykke.com/swagger/ui)
* [Hft API Definition](https://hft-api.lykke.com/swagger/ui/)
* [Hft API v2 Definition](https://hft-apiv2.lykke.com/swagger/ui)


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

