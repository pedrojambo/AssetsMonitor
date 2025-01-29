# AssetsMonitor

## Descrição

O AssetsMonitor é uma aplicação .NET que monitora cotações de ativos financeiros e envia alertas por e-mail quando os preços atingem determinados valores. A aplicação utiliza a API AlphaVantage para obter cotações de ativos e envia e-mails usando um serviço de envio de e-mails configurado. É importante salientar que a API Key gratuita da AlphaVantage permite apenas 25 consultas por dia.

## Configurações Necessárias

### appsettings.json

O arquivo `appsettings.json` deve ser configurado com as seguintes informações:
```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "SmtpSettings": {
        "Host": "smtp.gmail.com",
        "Port": 587,
        "Username": "your-user@gmail.com",
        "Password": "your-google-app-password",
        "EnableSsl": true
    },
    "AlertSettings": {
        "RecipientEmail": "your-recipient@gmail.com",
        "TemplatePath": "Templates/AlertEmailTemplate.html",
        "BrokerLink": "https://conteudos.xpi.com.br/acoes/",
        "DetailsLink": "https://www.alphavantage.co"
    },
    "AlphaVantageApiSettings": {
        "BaseUrl": "https://www.alphavantage.co/query",
        "ApiKey": "your-alphavantage-apikey",
        "RegionalSufix": "SA"
    }
}
```
### Configurando a API da AlphaVantage

Para obter a API Key da AlphaVantage e configurar a mesma, siga os passos abaixo:

1. Acesse o site [AlphaVantage](https://www.alphavantage.co/).
2. Clique em "Get Your Free API Key".
3. Preencha o formulário de registro e siga as instruções para obter sua API Key.
4. Substitua `"your-alphavantage-apikey"` no `appsettings.json` pela sua API Key.
5. Substitua `"RegionalSufix"` pelo sufixo regional usado pela API. O sufixo para ativos na bolsa brasileira é "SA".

### Gerando um App Password do Google 

Para gerar um App Password do Google e configurar o destinatário, siga os passos abaixo:

1. Escolha uma conta Google a ser utilizada para envio de email e substitua `"your-user@gmail.com"` pelo endereço de email da conta.
2. Acesse sua conta do Google e vá para [Segurança](https://myaccount.google.com/security).
3. Em "Fazer login no Google", garanta que a verificação de dois fatores está em uso. Se não estiver, ative-a.
4. Procure "app password" na barra de pesquisa das configurações e clique no resultado.
5. Digite um nome para a senha do app, como "AssetsMonitor".
6. Clique em "Gerar".
7. Copie a senha gerada e substitua `"your-google-app-password"` no `appsettings.json` pela senha gerada.
8. Substitua `"your-recipient@gmail.com"` pelo endereço do destinatário dos emails de alerta (pode ser de qualquer domínio).

## Executando a Aplicação

Para compilar a aplicação, o comando `dotnet publish` pode ser usado para compilar o programa para um diretório da sua escolha. Uma vez no diretório do programa, abra um terminal e use o seguinte comando no terminal para executar o programa:

```
./AssetsMonitor <symbol> <sellPrice> <buyPrice>
```

### Exemplo
```
./AssetsMonitor PETR4 37.62 36.59
```
Este comando iniciará a aplicação monitorando o ativo `PETR4` com um preço de venda de `37.62` e um preço de compra de `36.59`.

## Endpoints da API

A aplicação também expõe um endpoint de API que pode ser acessado via HTTP, uma vez que a aplicação está em execução.

### Consultar Histórico de Cotações

- **Método:** GET
- **URL:** `http://localhost:5000/api/Assets/getHistory?symbol=<symbol>`

### Exemplo
```
http://localhost:5000/api/Assets/getHistory?symbol=PETR4
```
