# AmadonStandardLib.Tests

Projeto de testes unitários para a biblioteca AmadonStandardLib, com foco especial na classe `LuceneBookSearch`.

## Estrutura do Projeto

- **LuceneBookSearchTests.cs**: Testes abrangentes para a funcionalidade de busca Lucene

## Configuração

### Pré-requisitos

- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code com extensão C#

### Instalação

```bash
dotnet restore
dotnet build
```

## Executando os Testes

### Via linha de comando:

```bash
# Executar todos os testes
dotnet test

# Executar com saída detalhada
dotnet test --logger "console;verbosity=detailed"

# Executar testes específicos
dotnet test --filter "FullyQualifiedName~LuceneBookSearch"
```

### Via Visual Studio:

1. Abra o Test Explorer (Test > Test Explorer)
2. Clique em "Run All" para executar todos os testes

## Estrutura dos Testes

### LuceneBookSearchTests

Os testes estão organizados nas seguintes categorias:

1. **Testes de Criação de Índice**
   - Verificação da criação do índice Lucene
   - Validação de não recriação quando já existe

2. **Testes de Busca Simples**
   - Busca por termos simples
   - Busca por frases exatas
   - Busca por termos inexistentes

3. **Testes de Operadores Booleanos**
   - Operador AND
   - Operador OR
   - Operador NOT

4. **Testes de Wildcards**
   - Wildcard asterisco (*)
   - Wildcard interrogação (?)

5. **Testes de Filtros por Paper**
   - Filtro por paper atual
   - Filtro por partes do livro

6. **Testes de Proximidade**
   - Busca com operador de proximidade (~)

7. **Testes de Tratamento de Erros**
   - Tratamento de dados nulos
   - Tratamento de queries inválidas

8. **Testes de Extração de Termos**
   - Extração de palavras para highlight

9. **Testes de Atualização de Índice**
   - Atualização de parágrafos no índice

10. **Testes de Limites e Performance**
    - Limite de resultados
    - Performance de busca

11. **Testes de Casos Especiais**
    - Caracteres especiais
    - Parágrafos divisores

## Ajustes Necessários

### IMPORTANTE: Dados de Teste

Os testes utilizam dados simulados (mock). Para testes mais realistas, você pode:

1. **Usar dados reais**: Modifique o método `CreateMockTranslation()` para carregar dados reais do Urantia Book
   
2. **Ajustar caminhos**: Os testes criam diretórios temporários automaticamente, mas você pode precisar ajustar:
   - Caminhos de índice
   - Caminhos de arquivos de dados

3. **Configurar StaticObjects**: Se os testes precisarem de configuração específica do `StaticObjects.Logger`, adicione no construtor:

```csharp
public LuceneBookSearchTests()
{
    // Configurar StaticObjects se necessário
    // StaticObjects.Logger = new YourLogger();
    
    _testIndexPath = Path.Combine(Path.GetTempPath(), "LuceneBookSearchTests_" + Guid.NewGuid().ToString());
    // ...
}
```

## Problemas Conhecidos e Melhorias

### Problemas Identificados no LuceneBookSearch.cs

1. **Uso inconsistente de Field ID**: 
   - No método `CreateUBIndex`: `FieldId` é usado
   - No método `UpdateIndex`: usa `"id"` (string literal)
   - **Correção sugerida**: Usar sempre `FieldId` constante

2. **Falta de validação de entrada**:
   - Método `UpdateIndex` não valida se o parágrafo é nulo
   - **Correção sugerida**: Adicionar validação

3. **Gerenciamento de recursos**:
   - `IndexWriter` é criado e descartado várias vezes
   - **Correção sugerida**: Reutilizar quando possível

4. **Tratamento de erros**:
   - Alguns métodos não tratam todas as exceções possíveis
   - **Correção sugerida**: Adicionar try-catch específicos

### Melhorias Sugeridas

1. **Cache de IndexSearcher**: Manter o searcher em cache para melhor performance
2. **Validação de queries**: Validar sintaxe antes de executar
3. **Logging mais detalhado**: Adicionar mais pontos de log
4. **Testes de integração**: Adicionar testes com dados reais completos

## Cobertura de Testes

Os testes atuais cobrem:
- ? Criação e gerenciamento de índice
- ? Buscas simples e complexas
- ? Operadores booleanos
- ? Wildcards
- ? Filtros
- ? Atualização de índice
- ? Tratamento de erros
- ?? Performance (testes básicos)
- ? Testes de concorrência (não implementados)
- ? Testes de stress com grande volume de dados

## Contribuindo

Para adicionar novos testes:

1. Crie um método de teste com atributo `[Fact]`
2. Use o padrão Arrange-Act-Assert
3. Use FluentAssertions para assertions mais legíveis
4. Adicione comentários explicando o propósito do teste

## Exemplo de Teste

```csharp
[Fact]
public void Execute_DeveFazerAlgo_QuandoCondicao()
{
    // Arrange (Preparar)
    _searchData.QueryString = "test";

    // Act (Executar)
    var result = _luceneSearch.Execute(_searchData);

    // Assert (Verificar)
    result.Should().BeTrue();
    _searchData.SearchResults.Should().NotBeEmpty();
}
```

## Recursos

- [Lucene.NET Documentation](https://lucenenet.apache.org/)
- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Moq Documentation](https://github.com/moq/moq4)
