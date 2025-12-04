# Wolverine.Kafka Pub/Sub Example

Este projeto demonstra como implementar o padrão Publisher/Subscriber usando **Wolverine** com transporte **Kafka**.

## 📋 Pré-requisitos

- .NET 10.0 ou superior
- Docker (para executar Kafka localmente)
- PowerShell ou terminal compatível

## 🚀 Iniciando o Kafka

Para testar o exemplo, você precisa de um servidor Kafka rodando. A maneira mais fácil é usar Docker:

```powershell
# Executar Kafka usando a imagem oficial Apache Kafka
docker run -d --name kafka -p 9092:9092 apache/kafka:latest
```

Para verificar se o Kafka está rodando:

```powershell
docker ps | Select-String kafka
```

## 🏗️ Arquitetura

O projeto consiste em:

### Messages
- **OrderCreated** - Record que representa um evento de pedido criado
  - `OrderId` (Guid)
  - `CustomerName` (string)
  - `Amount` (decimal)
  - `CreatedAt` (DateTime)

### Handlers
- **OrderCreatedHandler** - Processa mensagens `OrderCreated` recebidas do Kafka
  - Wolverine auto-descobre handlers baseado em convenções
  - O método `Handle(OrderCreated message)` é chamado automaticamente

### Services
- **OrderPublisher** - Serviço que publica mensagens para o Kafka
  - Injeta `IMessageBus` do Wolverine
  - Método `PublishOrderAsync()` para enviar mensagens

### Worker
- **Worker** - BackgroundService que demonstra o uso
  - Publica mensagens de exemplo a cada 5 segundos
  - Usa nomes de clientes aleatórios e valores aleatórios

## 🔧 Configuração

O arquivo `appsettings.json` contém as configurações do Kafka:

```json
{
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    "TopicName": "wolverine-orders",
    "GroupId": "wolverine-consumer-group"
  }
}
```

Você pode modificar essas configurações conforme necessário.

## ▶️ Executando a Aplicação

```powershell
# Restaurar pacotes
dotnet restore

# Compilar o projeto
dotnet build

# Executar a aplicação
dotnet run --project Wolverine\Wolverine.csproj
```

## 📊 Saída Esperada

Quando a aplicação estiver rodando, você verá logs como:

```
info: Wolverine.Worker[0]
      Worker started. Publishing orders every 5 seconds...
info: Wolverine.Services.OrderPublisher[0]
      Publishing order: a3f2e8d1-... for customer: Alice
info: Wolverine.Handlers.OrderCreatedHandler[0]
      Processing order: a3f2e8d1-... for customer: Alice with amount: $542
```

Isso demonstra que:
1. O Worker está publicando mensagens no Kafka
2. O Wolverine está roteando as mensagens através do Kafka
3. O Handler está recebendo e processando as mensagens

## 🛑 Parando os Serviços

Para parar a aplicação:
- Pressione `Ctrl+C` no terminal

Para parar e remover o container Kafka:

```powershell
docker stop kafka
docker rm kafka
```

## 📚 Recursos Adicionais

- [Documentação do Wolverine](https://wolverine.netlify.app/)
- [Wolverine Kafka Transport](https://wolverine.netlify.app/guide/messaging/transports/kafka.html)
- [Apache Kafka](https://kafka.apache.org/)

## 🔍 Detalhes de Implementação

### Como o Wolverine Funciona

1. **Auto-Discovery**: Wolverine automaticamente descobre handlers no assembly
2. **Routing**: Mensagens publicadas via `IMessageBus.PublishAsync()` são roteadas conforme configuração
3. **Kafka Integration**: O transporte Kafka serializa/deserializa mensagens automaticamente
4. **Inline Processing**: `.ProcessInline()` processa mensagens na mesma thread (ideal para testes)

### Configuração do Program.cs

```csharp
builder.Host.UseWolverine(opts =>
{
    // Configura o transporte Kafka
    opts.UseKafka("localhost:9092");

    // Define rota de publicação
    opts.PublishMessage<OrderCreated>()
        .ToKafkaTopic("wolverine-orders");

    // Define assinatura do tópico
    opts.ListenToKafkaTopic("wolverine-orders")
        .ProcessInline();
});
```

## 🎯 Próximos Passos

- Implementar persistência de mensagens com Entity Framework Core
- Adicionar retry policies e dead letter queues
- Implementar múltiplos consumers em diferentes processos
- Adicionar validação de mensagens
- Implementar testes de integração
