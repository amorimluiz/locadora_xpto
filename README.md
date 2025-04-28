# API LocadoraXpto

Este documento descreve todos os endpoints disponíveis na API **LocadoraXpto**, incluindo os parâmetros de query string, payloads de entrada e filtros avançados.

---

## Base URL
```
https://localhost:5001/api
```

---

## 1. Fabricantes

### Endpoints CRUD

- **GET** `/fabricantes?nome={nome}`
  - Filtra (opcional) pela parte do nome.
  - **200 OK**: retorna lista de fabricantes.

- **GET** `/fabricantes/{id}`
  - Busca fabricante por ID.
  - **200 OK**: retorna objeto `{ "fabricanteId": 1, "nome": "Fiat" }`
  - **404 Not Found**: se não existir.

- **POST** `/fabricantes`
  - Cria um novo fabricante.
  - **Payload**:
    ```json
    {
      "nome": "Fiat"
    }
    ```
  - **201 Created**: retorna objeto criado.
  - **400 Bad Request**: se `nome` faltar ou exceder 100 caracteres.

- **PUT** `/fabricantes/{id}`
  - Atualiza o nome de um fabricante.
  - **Payload**:
    ```json
    {
      "fabricanteId": 1,
      "nome": "Volkswagen"
    }
    ```
  - **204 No Content**: sucesso.
  - **400 Bad Request**: ID não confere ou payload inválido.
  - **404 Not Found**: se não existir.

- **DELETE** `/fabricantes/{id}`
  - Remove fabricante.
  - **204 No Content**: sucesso.
  - **404 Not Found**: se não existir.

### Filtro Avançado

- **GET** `/fabricantes/media-diaria`
  - Calcula média de valor diário de aluguel por fabricante.
  - **200 OK**: lista de objetos
    ```json
    [
      { "fabricante": "Fiat", "mediaDiaria": 120.50 },
      { "fabricante": "Volkswagen", "mediaDiaria": 98.00 }
    ]
    ```

---

## 2. Veículos

### Endpoints CRUD

- **GET** `/veiculos?fabricanteId={id}&ano={ano}&kmMin={quilometragem}`
  - Filtros opcionais:
    - `fabricanteId` (int)
    - `ano` (int)
    - `kmMin` (int)
  - **200 OK**: retorna lista de veículos com fabricante incluído.

- **GET** `/veiculos/{id}`
  - Busca veículo por ID.
  - **200 OK** com JSON de veículo.
  - **404 Not Found** se não existir.

- **POST** `/veiculos`
  - Cria veículo.
  - **Payload**:
    ```json
    {
      "modelo": "Gol",
      "anoFabricacao": 2020,
      "quilometragem": 15000,
      "fabricanteId": 1
    }
    ```
  - **201 Created**: objeto criado.
  - **400 Bad Request**: campos obrigatórios faltando.

- **PUT** `/veiculos/{id}`
  - Atualiza veículo.
  - **Payload**:
    ```json
    {
      "veiculoId": 1,
      "modelo": "Onix",
      "anoFabricacao": 2021,
      "quilometragem": 12000,
      "fabricanteId": 1
    }
    ```
  - **204 No Content**, **400**, **404** conforme validação.

- **DELETE** `/veiculos/{id}`
  - Remove veículo.
  - **204 No Content** ou **404 Not Found**.

### Filtro Avançado

- **GET** `/veiculos/com-alugueis?minAlugueis={quantidade}`
  - Lista veículos com pelo menos `minAlugueis` aluguéis.
  - **200 OK**:
    ```json
    [
      { "veiculoId": 1, "modelo": "Gol", "totalAlugueis": 3 },
      { "veiculoId": 2, "modelo": "Onix", "totalAlugueis": 2 }
    ]
    ```

---

## 3. Clientes

### Endpoints CRUD

- **GET** `/clientes?nome={nome}`
  - Filtra por parte do nome.
  - **200 OK**: lista de clientes.

- **GET** `/clientes/{id}`
  - **200 OK** ou **404 Not Found**.

- **POST** `/clientes`
  - **Payload**:
    ```json
    {
      "nome": "Ana Silva",
      "cpf": "12345678901",
      "email": "ana@exemplo.com"
    }
    ```
  - **201 Created** ou **400 Bad Request**.

- **PUT** `/clientes/{id}`
  - **Payload**:
    ```json
    {
      "clienteId": 1,
      "nome": "Ana S. Silva",
      "cpf": "12345678901",
      "email": "ana.s@exemplo.com"
    }
    ```
  - **204**, **400**, **404**.

- **DELETE** `/clientes/{id}`
  - **204** ou **404**.

### Filtro Avançado

- **GET** `/clientes/contagem-alugueis`
  - Conta quantos aluguéis cada cliente fez.
  - **200 OK**:
    ```json
    [
      { "clienteId": 1, "nome": "Ana Silva", "totalAlugueis": 4 },
      { "clienteId": 2, "nome": "João Silva", "totalAlugueis": 1 }
    ]
    ```

---

## 4. Aluguéis

### Endpoints CRUD

- **GET** `/alugueis?clienteId={id}&ativo={true|false}&valorMin={valor}`
  - Filtros opcionais.
  - **200 OK**.

- **GET** `/alugueis/{id}` → **200** ou **404**.

- **POST** `/alugueis`
  - **Payload**:
    ```json
    {
      "clienteId": 1,
      "veiculoId": 1,
      "dataInicio": "2025-04-27T10:00:00",
      "dataFim": "2025-04-30T10:00:00",
      "quilometragemInicial": 15000,
      "valorDiaria": 100.00
    }
    ```
  - **201 Created** ou **400 Bad Request**.

- **PUT** `/alugueis/{id}`
  - Atualiza, incluindo `dataDevolucao`, `quilometragemFinal` e `valorTotal`.
  - **204**, **400**, **404**.

- **DELETE** `/alugueis/{id}` → **204** ou **404**.

### Filtro Avançado

- **GET** `/alugueis/filtro-avancado?de={yyyy-MM-dd}&ate={yyyy-MM-dd}&valorMin={valor}&ativo={true|false}`
  - Combina data, valor e status de devolução.
  - **200 OK** lista de objetos `Aluguel`.

---

## 5. Pagamentos

### Endpoints CRUD

- **GET** `/pagamentos?aluguelId={id}` → lista de pagamentos.
- **GET** `/pagamentos/{id}` → **200** ou **404**.

- **POST** `/pagamentos`
  - **Payload**:
    ```json
    {
      "aluguelId": 1,
      "dataPagamento": "2025-04-28T15:00:00",
      "valorPago": 300.00,
      "metodoPagamento": "Cartão de Crédito"
    }
    ```
  - **201** ou **400**.

- **PUT** `/pagamentos/{id}`
  - Atualiza dados de pagamento.
  - **204**, **400**, **404**.

- **DELETE** `/pagamentos/{id}` → **204** ou **404**.

### Filtro Avançado

- **GET** `/pagamentos/com-aluguel?minPago={valor}`
  - Retorna pagamentos com valor maior ou igual a `minPago`, incluindo data de início e fim do aluguel.

---

**Observações finais:**
- Todos os endpoints esperam cabeçalho `Content-Type: application/json` nas requisições POST/PUT.
- Queries podem omitir parâmetros opcionais para retornar tudo.
- Use o Swagger UI (`/swagger`) para testar interativamente e ver exemplos de resposta.