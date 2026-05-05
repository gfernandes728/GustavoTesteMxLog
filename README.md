# GustavoTesteMxLog

Projeto ASP.NET Core API para cadastro, atualização, exclusão e visualização de Usuários cadastrados.

---

## 📁 Estrutura do Projeto

```
GustavoTesteMxLog/
├── GustavoTesteMxLog/					# Projeto principal API
├── GustavoTesteMxLog.Application/		# Aplicação e Regras de negócio
├── GustavoTesteMxLog.Infra/			# Repositórios e DbContext
├── GustavoTesteMxLog.Domain/			# Entidades
├── GustavoTesteMxLog.UnitTests/		# Testes unitários
└── appsettings.json					# Configurações
```

---

## 🚀 Como rodar o projeto localmente

### 1. Clonar o repositório

```bash
git clone https://github.com/gfernandes728/GustavoTesteMxLog.git
cd GustavoTesteMxLog
```

### 2. Restaurar os pacotes

```bash
dotnet restore
```

### 3. Configurar o banco de dados

Edite o arquivo `GustavoTesteMxLog/appsettings.json` com sua string de conexão:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MxLogDb;Trusted_Connection=True;"
}
```

### 4. Aplicar as migrations

Para criar uma nova migration inicial (se necessário):

```bash
dotnet ef migrations add InitialCreate --project GustavoTesteMxLog.Infra --startup-project GustavoTesteMxLog
```

Para aplicar a migration existente:

```bash
dotnet ef database update --project GustavoTesteMxLog.Infra --startup-project GustavoTesteMxLog
```

> 💡 Certifique-se de que o pacote `Microsoft.EntityFrameworkCore.Tools` esteja instalado.

### 5. Rodar o projeto

```bash
dotnet run --project GustavoTesteMxLog --launch-profile "https"
```
Acesse no navegador: [https://localhost:7182/swagger/index.html](https://localhost:7182/swagger/index.html)

> 💡 Na primeira vez que se roda o projeto será feito uma carga inicial de um Usuário para ser acessado, para conseguir logar e fazer o cadastro dos demais.

```
O Usuário inicial cadastrado será este:

Nome:	admin_mxlog
E-mail:	admin@mxlog.com.br
Senha:	admin123
```

> 💡 O E-mail do Usuário inicial não poderá ser alterado, apenas seu nome e senha.
> 💡 O Usuário inicial não poderá ser excluído.
> 💡 O Usuário logado não poderá ser excluído.
---

## ✅ Rodar os Testes

```bash
dotnet test GustavoTesteMxLog.UnitTests
```

---

## 📦 Publicação

```bash
dotnet publish -c Release -o ./publish
```

---

## 👤 Autor

**Gustavo Fernandes**  
📧 guga.728@gmail.com  
