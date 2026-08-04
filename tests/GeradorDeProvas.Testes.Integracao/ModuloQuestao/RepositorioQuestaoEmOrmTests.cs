using FizzWare.NBuilder;
using GeradorDeProvas.Dominio.Modulos.ModuloDisciplina;
using GeradorDeProvas.Dominio.Modulos.ModuloMateria;
using GeradorDeProvas.Dominio.Modulos.ModuloQuestao;
using GeradorDeProvas.Testes.Integracao.Compartilhado.Orm;

namespace GeradorDeProvas.Testes.Integracao.ModuloQuestao;

[TestClass]
public sealed class RepositorioQuestaoEmOrmTests : RepositorioBaseEmOrmTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRegistro_ComRelacionamentos()
    {
        Disciplina disciplina = Builder<Disciplina>
            .CreateNew()
            .With(d => d.UserId = Guid.Empty)
            .Persist();

        Materia materia = Builder<Materia>
            .CreateNew()
            .With(m => m.Disciplina = disciplina)
            .With(m => m.UserId = Guid.Empty)
            .Persist();

        Questao questao = Builder<Questao>
            .CreateNew()
            .With(q => q.Enunciado = "Enunciado1")
            .With(q => q.Materia = materia)
            .With(q => q.Alternativas = CriarAlternativas())
            .With(q => q.UserId = Guid.Empty)
            .Build();

        repositorioQuestao.Cadastrar(questao);
        dbContext.ChangeTracker.Clear();

        Questao? questaoSelecionada = repositorioQuestao.SelecionarPorId(questao.Id);

        Assert.IsNotNull(questaoSelecionada);
        Assert.AreEqual("Enunciado1", questaoSelecionada.Enunciado);
        Assert.AreEqual(materia.Id, questaoSelecionada.Materia.Id);
        Assert.HasCount(2, questaoSelecionada.Alternativas);
    }

    [TestMethod]
    public void Editar_AtualizaRegistroExistente()
    {
        Disciplina disciplina = Builder<Disciplina>
            .CreateNew()
            .With(d => d.UserId = Guid.Empty)
            .Persist();

        Materia materia = Builder<Materia>
            .CreateNew()
            .With(m => m.Disciplina = disciplina)
            .With(m => m.UserId = Guid.Empty)
            .Persist();

        Questao questao = Builder<Questao>
            .CreateNew()
            .With(q => q.Enunciado = "Enunciado1")
            .With(q => q.Materia = materia)
            .With(q => q.Alternativas = CriarAlternativas())
            .With(q => q.UserId = Guid.Empty)
            .Persist();

        Disciplina disciplinaAtualizada = Builder<Disciplina>
            .CreateNew()
            .With(d => d.UserId = Guid.Empty)
            .Persist();

        Materia materiaAtualizada = Builder<Materia>
            .CreateNew()
            .With(m => m.Disciplina = disciplinaAtualizada)
            .With(m => m.UserId = Guid.Empty)
            .Persist();

        Questao questaoAtualizada = Builder<Questao>
            .CreateNew()
            .With(q => q.Enunciado = "EnunciadoAtualizado")
            .With(q => q.Materia = materiaAtualizada)
            .With(q => q.Alternativas = CriarAlternativas())
            .With(q => q.UserId = Guid.Empty)
            .Build();

        bool conseguiuEditar = repositorioQuestao.Editar(questao.Id, questaoAtualizada);
        dbContext.ChangeTracker.Clear();

        Questao? questaoSelecionada = repositorioQuestao.SelecionarPorId(questao.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(questaoSelecionada);
        Assert.AreEqual("EnunciadoAtualizado", questaoSelecionada.Enunciado);
        Assert.AreEqual(materiaAtualizada.Id, questaoSelecionada.Materia.Id);
        Assert.HasCount(2, questaoSelecionada.Alternativas);
    }

    [TestMethod]
    public void Excluir_RemoveRegistroExistente()
    {
        Disciplina disciplina = Builder<Disciplina>
            .CreateNew()
            .With(d => d.UserId = Guid.Empty)
            .Persist();

        Materia materia = Builder<Materia>
            .CreateNew()
            .With(m => m.Disciplina = disciplina)
            .With(m => m.UserId = Guid.Empty)
            .Persist();

        Questao questao = Builder<Questao>
            .CreateNew()
            .With(q => q.Enunciado = "Enunciado1")
            .With(q => q.Materia = materia)
            .With(q => q.Alternativas = CriarAlternativas())
            .With(q => q.UserId = Guid.Empty)
            .Persist();

        dbContext.ChangeTracker.Clear();

        bool conseguiuExcluir = repositorioQuestao.Excluir(questao.Id);
        dbContext.ChangeTracker.Clear();

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(repositorioQuestao.SelecionarPorId(questao.Id));
    }

    [TestMethod]
    public void SelecionarTodos_CarregaRegistros_ComRelacionamentos()
    {
        Disciplina disciplina = Builder<Disciplina>
            .CreateNew()
            .With(d => d.UserId = Guid.Empty)
            .Persist();

        Materia materia = Builder<Materia>
            .CreateNew()
            .With(m => m.Disciplina = disciplina)
            .With(m => m.UserId = Guid.Empty)
            .Persist();

        IList<Questao> questoes = Builder<Questao>
            .CreateListOfSize(3)
            .All()
            .With(q => q.Enunciado = "Enunciado1")
            .With(q => q.Materia = materia)
            .With(q => q.Alternativas = CriarAlternativas())
            .With(q => q.UserId = Guid.Empty)
            .Persist();

        dbContext.ChangeTracker.Clear();

        List<Questao> questoesSelecionadas = repositorioQuestao.SelecionarTodos();

        Assert.HasCount(3, questoesSelecionadas);
        CollectionAssert.AreEquivalent(
            questoes.Select(q => q.Id).ToList(),
            questoesSelecionadas.Select(q => q.Id).ToList()
        );
        Assert.IsTrue(questoesSelecionadas.All(q => q.Materia.Id == materia.Id));
        Assert.IsTrue(questoesSelecionadas.All(q => q.Alternativas.Count == 2));
    }

    private static List<Alternativa> CriarAlternativas()
    {
        return
        [
            new Alternativa("Alternativa 1", false),
            new Alternativa("Alternativa 2", true)
        ];
    }
}
