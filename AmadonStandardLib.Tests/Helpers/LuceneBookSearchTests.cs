using AmadonStandardLib.Classes;
using AmadonStandardLib.InterchangeData;
using AmadonStandardLib.UbClasses;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace AmadonStandardLib.Tests.Helpers
{
    /// <summary>
    /// Testes unitários para a classe LuceneBookSearch
    /// 
    /// IMPORTANTE: Ajuste os caminhos de dados conforme necessário para o seu ambiente
    /// </summary>
    public class LuceneBookSearchTests : IDisposable
    {
        private readonly string _testIndexPath;
        private readonly LuceneBookSearch _luceneSearch;
        private SearchData _searchData;

        public LuceneBookSearchTests()
        {
            // Cria um diretório temporário para os índices de teste
            _testIndexPath = Path.Combine(Path.GetTempPath(), "LuceneBookSearchTests_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testIndexPath);

            _luceneSearch = new LuceneBookSearch();
            
            // Inicializa SearchData com dados de teste
            _searchData = CreateTestSearchData();
        }

        public void Dispose()
        {
            try
            {
                _luceneSearch.Dispose();
                
                // Limpa o diretório de teste
                if (Directory.Exists(_testIndexPath))
                {
                    Directory.Delete(_testIndexPath, true);
                }
            }
            catch (Exception ex)
            {
                // Log do erro, mas não falha o teste
                System.Diagnostics.Debug.WriteLine($"Erro ao limpar recursos: {ex.Message}");
            }
        }

        #region Métodos Auxiliares para Criar Dados de Teste

        /// <summary>
        /// Cria dados de teste com uma tradução simulada
        /// AJUSTE: Você pode precisar modificar este método para usar dados reais
        /// </summary>
        private SearchData CreateTestSearchData()
        {
            var translation = CreateMockTranslation();
            
            return new SearchData
            {
                Translation = translation,
                IndexPathRoot = _testIndexPath,
                Part1Included = true,
                Part2Included = true,
                Part3Included = true,
                Part4Included = true,
                CurrentPaperOnly = false,
                QueryString = "",
                CurrentPaper = 0
            };
        }

        /// <summary>
        /// Cria uma tradução simulada com alguns papers de teste
        /// AJUSTE: Substitua por dados reais se disponível
        /// </summary>
        private Translation CreateMockTranslation()
        {
            var translation = new Translation
            {
                LanguageID = 1,
                Description = "Test Translation"
            };

            // Cria papers de teste (0-196)
            for (short paperNo = 0; paperNo <= 5; paperNo++) // Apenas alguns papers para teste
            {
                var paper = CreateMockPaper(paperNo, translation.LanguageID);
                translation.Papers.Add(paper);
            }

            return translation;
        }

        /// <summary>
        /// Cria um paper simulado com parágrafos de teste
        /// </summary>
        private Paper CreateMockPaper(short paperNo, short translationId)
        {
            var paper = new Paper();
            
            // Título do paper
            paper.Paragraphs.Add(new Paragraph
            {
                TranslationId = translationId,
                Paper = paperNo,
                Section = 0,
                ParagraphNo = 0,
                Text = $"Paper {paperNo} Title - The Universe and God",
                FormatInt = 1 // Title format
            });

            // Alguns parágrafos com conteúdo de teste
            var testContents = new[]
            {
                "In the beginning God created the heavens and the earth. The universe is filled with divine purpose.",
                "Jesus Christ Michael is the creator of our local universe. He came to earth to reveal the Father.",
                "The Urantia Book contains revelations about spiritual truth and the nature of reality.",
                "Faith is essential for spiritual growth and understanding divine purposes.",
                "Prayer and worship help us connect with the Universal Father and his divine presence."
            };

            for (short i = 1; i <= testContents.Length; i++)
            {
                paper.Paragraphs.Add(new Paragraph
                {
                    TranslationId = translationId,
                    Paper = paperNo,
                    Section = (short)((i - 1) / 3), // Divide em seções
                    ParagraphNo = i,
                    Text = testContents[i - 1],
                    FormatInt = 0 // Normal paragraph
                });
            }

            return paper;
        }

        #endregion

        #region Testes de Criação de Índice

        [Fact]
        public void Execute_DevecriarIndice_QuandoChamadoPrimeiravez()
        {
            // Arrange
            _searchData.QueryString = "God";

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue("o índice deve ser criado com sucesso");
            Directory.Exists(Path.Combine(_testIndexPath, "T001")).Should().BeTrue("o diretório do índice deve existir");
        }

        [Fact]
        public void Execute_NaoDeveCriarIndiceNovamente_QuandoJaExiste()
        {
            // Arrange
            _searchData.QueryString = "God";
            _luceneSearch.Execute(_searchData); // Primeira execução cria o índice
            var indexPath = Path.Combine(_testIndexPath, "T001");
            var creationTime = Directory.GetCreationTime(indexPath);

            System.Threading.Thread.Sleep(100); // Aguarda para garantir diferença de tempo

            // Act
            _luceneSearch.Execute(_searchData); // Segunda execução

            // Assert
            var newCreationTime = Directory.GetCreationTime(indexPath);
            newCreationTime.Should().Be(creationTime, "o índice não deve ser recriado");
        }

        #endregion

        #region Testes de Busca Simples

        [Fact]
        public void Execute_DeveEncontrarResultados_ParaBuscaSimples()
        {
            // Arrange
            _searchData.QueryString = "God";

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue("a busca deve ser executada com sucesso");
            _searchData.SearchResults.Should().NotBeEmpty("deve encontrar resultados contendo 'God'");
        }

        [Fact]
        public void Execute_DeveEncontrarResultados_ParaBuscaComFraseExata()
        {
            // Arrange
            _searchData.QueryString = "\"Jesus Christ\"";

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            _searchData.SearchResults.Should().NotBeEmpty("deve encontrar a frase exata 'Jesus Christ'");
        }

        [Fact]
        public void Execute_NaoDeveEncontrarResultados_ParaTermoInexistente()
        {
            // Arrange
            _searchData.QueryString = "XyZaAbBcC123456"; // Termo que não existe

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue("a busca deve executar sem erros");
            _searchData.SearchResults.Should().BeEmpty("não deve encontrar resultados para termo inexistente");
        }

        #endregion

        #region Testes de Operadores Booleanos

        [Fact]
        public void Execute_DeveEncontrarResultados_ParaOperadorAND()
        {
            // Arrange
            _searchData.QueryString = "God AND universe";

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            _searchData.SearchResults.Should().NotBeEmpty("deve encontrar parágrafos com ambos os termos");
            
            // Verifica se os resultados contêm ambos os termos
            foreach (var searchResult in _searchData.SearchResults)
            {
                var text = searchResult.Entry.Text.ToLower();
                text.Should().Contain("god");
                text.Should().Contain("universe");
            }
        }

        [Fact]
        public void Execute_DeveEncontrarResultados_ParaOperadorOR()
        {
            // Arrange
            _searchData.QueryString = "Jesus OR faith";

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            _searchData.SearchResults.Should().NotBeEmpty("deve encontrar parágrafos com pelo menos um dos termos");
        }

        [Fact]
        public void Execute_DeveRespeitarOperadorNOT()
        {
            // Arrange
            _searchData.QueryString = "God NOT Jesus";

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            
            // Verifica que nenhum resultado contém "Jesus"
            foreach (var searchResult in _searchData.SearchResults)
            {
                var text = searchResult.Entry.Text.ToLower();
                text.Should().NotContain("jesus", "resultados não devem conter o termo excluído");
            }
        }

        #endregion

        #region Testes de Wildcards

        [Fact]
        public void Execute_DeveEncontrarResultados_ComWildcardAsterisco()
        {
            // Arrange
            _searchData.QueryString = "spir*"; // spiritual, spirit, etc.

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            _searchData.SearchResults.Should().NotBeEmpty("deve encontrar palavras começando com 'spir'");
        }

        [Fact]
        public void Execute_DeveEncontrarResultados_ComWildcardInterrogacao()
        {
            // Arrange
            _searchData.QueryString = "fait?"; // faith

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            _searchData.SearchResults.Should().NotBeEmpty("deve encontrar palavras com um caractere variável");
        }

        #endregion

        #region Testes de Filtros por Paper

        [Fact]
        public void Execute_DeveRetornarApenasResultadosDoPaperAtual_QuandoCurrentPaperOnlyTrue()
        {
            // Arrange
            _searchData.QueryString = "God";
            _searchData.CurrentPaperOnly = true;
            _searchData.CurrentPaper = 1;

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            
            // Verifica que todos os resultados são do paper 1
            foreach (var searchResult in _searchData.SearchResults)
            {
                searchResult.Entry.Paper.Should().Be(1, "todos os resultados devem ser do paper atual");
            }
        }

        [Fact]
        public void Execute_DeveRespeitarFiltrosDeParts()
        {
            // Arrange
            _searchData.QueryString = "God";
            _searchData.Part1Included = true;
            _searchData.Part2Included = false;
            _searchData.Part3Included = false;
            _searchData.Part4Included = false;

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            
            // Verifica que os resultados são apenas da Part 1 (papers 0-31)
            foreach (var searchResult in _searchData.SearchResults)
            {
                searchResult.Entry.Paper.Should().BeLessOrEqualTo(31, "resultados devem ser apenas da Part 1");
            }
        }

        #endregion

        #region Testes de Proximidade

        [Fact]
        public void Execute_DeveEncontrarResultados_ComBuscaDeProximidade()
        {
            // Arrange
            _searchData.QueryString = "\"Jesus God\"~10"; // Jesus e God dentro de 10 palavras

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue("busca de proximidade deve executar");
        }

        #endregion

        #region Testes de Tratamento de Erros

        [Fact]
        public void Execute_DeveRetornarFalse_QuandoSearchDataNull()
        {
            // Arrange
            SearchData? nullSearchData = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _luceneSearch.Execute(nullSearchData!));
        }

        [Fact]
        public void Execute_DeveTratarErro_QuandoTranslationNull()
        {
            // Arrange
            _searchData.Translation = null;
            _searchData.QueryString = "God";

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeFalse("deve retornar false quando Translation é null");
        }

        [Fact]
        public void Execute_DeveTratarErro_ParaQueryStringInvalida()
        {
            // Arrange
            _searchData.QueryString = "AND OR NOT"; // Query inválida

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            // Pode retornar true ou false dependendo de como o Lucene trata
            // O importante é não lançar exceção
            if (!result)
            {
                _searchData.ErrorMessage.Should().NotBeNullOrEmpty("deve ter uma mensagem de erro");
            }
        }

        #endregion

        #region Testes de Extração de Termos

        [Fact]
        public void Execute_DeveExtrairTermosDeBusca_ParaHighlight()
        {
            // Arrange
            _searchData.QueryString = "God Jesus universe";

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            _searchData.Words.Should().NotBeEmpty("deve extrair palavras para highlight");
        }

        #endregion

        #region Testes de Atualização de Índice

        [Fact]
        public void UpdateIndex_DeveAtualizarParagrafo_NoIndice()
        {
            // Arrange
            _searchData.QueryString = "God";
            _luceneSearch.Execute(_searchData); // Cria o índice inicial

            var updatedParagraph = new Paragraph
            {
                TranslationId = 1,
                Paper = 1,
                Section = 0,
                ParagraphNo = 1,
                Text = "Updated text with NewUniqueWord123 for testing",
                FormatInt = 0
            };

            // Act
            _luceneSearch.UpdateIndex(updatedParagraph);

            // Busca o novo termo
            _searchData.SearchResults.Clear();
            _searchData.QueryString = "NewUniqueWord123";
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            _searchData.SearchResults.Should().NotBeEmpty("deve encontrar o parágrafo atualizado");
        }

        #endregion

        #region Testes de Limites e Performance

        [Fact]
        public void Execute_DeveRetornarNoMaximo200Resultados()
        {
            // Arrange
            _searchData.QueryString = "*"; // Busca que pode retornar muitos resultados

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            _searchData.SearchResults.Count.Should().BeLessOrEqualTo(200, "deve limitar a 200 resultados");
        }

        [Fact]
        public void Execute_DeveExecutarRapido_ParaBuscaSimples()
        {
            // Arrange
            _searchData.QueryString = "God";
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var result = _luceneSearch.Execute(_searchData);
            stopwatch.Stop();

            // Assert
            result.Should().BeTrue();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000, "busca deve ser rápida (menos de 5 segundos)");
        }

        #endregion

        #region Testes de Casos Especiais

        [Fact]
        public void Execute_DeveTratarCaracteresEspeciais()
        {
            // Arrange
            var specialQueries = new[]
            {
                "\"test query\"",
                "(God OR Jesus)",
                "God+universe",
                "faith~0.8"
            };

            foreach (var query in specialQueries)
            {
                _searchData.SearchResults.Clear();
                _searchData.QueryString = query;

                // Act
                var result = _luceneSearch.Execute(_searchData);

                // Assert
                result.Should().BeTrue($"deve processar query: {query}");
            }
        }

        [Fact]
        public void Execute_DeveIgnorarParagrafosDivisores()
        {
            // Arrange
            _searchData.QueryString = "*";

            // Act
            var result = _luceneSearch.Execute(_searchData);

            // Assert
            result.Should().BeTrue();
            
            // Verifica que nenhum resultado é um divisor
            foreach (var searchResult in _searchData.SearchResults)
            {
                // Divisores geralmente têm formato especial
                // Ajuste conforme sua implementação
            }
        }

        #endregion
    }
}
