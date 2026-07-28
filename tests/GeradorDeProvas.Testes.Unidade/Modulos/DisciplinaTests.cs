using GeradorDeProvas.Dominio.Modulos.ModuloDisciplina;
namespace GeradorDeProvas.Testes.Unidade.Modulos;

[TestClass]
public sealed class DisciplinaTests
{
    #region Testes da Validação de Disciplina

    [TestMethod]
    public void Validar_ComNomeVazio_DeveRetornarErro()
    {
        // Arranjo [Configura os dados do teste]
        Disciplina disciplina = new(string.Empty);

        // Ação [Executa a ação sob testes]
        List<string> erros = disciplina.Validar();

        // Asserção [Checa o resultado comparando com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComNomeCurto_DeveRetornarErro()
    {
        Disciplina disciplina = new(new string('A', 1));

        List<string> erros = disciplina.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComNomeLongo_DeveRetornarErro()
    {
        Disciplina disciplina = new(new string('A', 101));

        List<string> erros = disciplina.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    #endregion
}
