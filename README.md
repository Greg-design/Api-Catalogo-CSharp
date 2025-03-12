# 🛍️ API Catálogo de Produtos

API REST desenvolvida em .NET 8 para gerenciamento de catálogo de produtos e suas categorias.

## 📋 Sobre o Projeto

Esta API foi desenvolvida seguindo as melhores práticas de desenvolvimento, utilizando o padrão REST e arquitetura em camadas. O projeto permite o gerenciamento completo de produtos e suas categorias, oferecendo endpoints para todas as operações CRUD (Create, Read, Update, Delete).

## 🚀 Principais Funcionalidades

- Gerenciamento completo de Categorias
- Gerenciamento completo de Produtos
- Relacionamento entre Produtos e Categorias
- Tratamento de exceções globalizado
- Logs de operações e erros
- Documentação via Swagger

## 🛠️ Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger/OpenAPI
- Logging

## 📦 Pré-requisitos

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 ou VS Code

## ⚙️ Configuração e Execução

1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/APICatalogo.git
```

2. Navegue até a pasta do projeto

```bash
cd APICatalogo
```

3. Restaure os pacotes

```bash
dotnet restore
```

4. Configure a string de conexão no arquivo `appsettings.json`

5. Execute as migrations

```bash
dotnet ef database update
```

6. Execute o projeto

```bash
dotnet run
```

## 🔄 Endpoints Disponíveis

### Categorias

- GET /Categorias - Lista todas as categorias
- GET /Categorias/{id} - Obtém uma categoria específica
- GET /Categorias/produtos - Lista todas as categorias com seus produtos
- POST /Categorias - Cria uma nova categoria
- PUT /Categorias/{id} - Atualiza uma categoria existente
- DELETE /Categorias/{id} - Remove uma categoria

## 📝 Documentação da API

Após executar o projeto, acesse a documentação completa da API através do Swagger:

```
https://localhost:7200/swagger
```

## 🤝 Contribuição

Contribuições são sempre bem-vindas! Para contribuir:

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## ✒️ Autor

Seu Nome - [Seu GitHub](https://github.com/seu-usuario)

---

⌨️ com ❤️ por Gregory
