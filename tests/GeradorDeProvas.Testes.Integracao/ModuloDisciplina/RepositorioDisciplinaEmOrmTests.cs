using FizzWare.NBuilder;
using GeradorDeProvas.Dominio.Modulos.ModuloDisciplina;
using GeradorDeProvas.Testes.Integracao.Compartilhado.Orm;

namespace GeradorDeProvas.Testes.Integracao.ModuloDisciplina;

[TestClass]
public sealed class RepositorioDisciplinaEmOrmTests : RepositorioBaseEmOrmTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRegistro()
    {
        Disciplina disciplina = Builder<Disciplina>
            .CreateNew()
            .With(d => d.UserId = Guid.Empty)
            .Build();

        repositorioDisciplina.Cadastrar(disciplina);
        dbContext.ChangeTracker.Clear();

        Disciplina? disciplinaSelecionada = repositorioDisciplina.SelecionarPorId(disciplina.Id);

        Assert.IsNotNull(disciplinaSelecionada);
        Assert.AreEqual("Nome1", disciplinaSelecionada.Nome);
    }

    [TestMethod]
    public void Editar_AtualizaRegistroExistente()
    {
        Disciplina disciplina = Builder<Disciplina>
            .CreateNew()
            .With(d => d.UserId = Guid.Empty)
            .Persist();

        Disciplina disciplinaAtualizada = Builder<Disciplina>
            .CreateNew()
            .With(d => d.Nome = "NomeAtualizado")
            .With(d => d.UserId = Guid.Empty)
            .Build();

        bool conseguiuEditar = repositorioDisciplina.Editar(disciplina.Id, disciplinaAtualizada);
        dbContext.ChangeTracker.Clear();

        Disciplina? disciplinaSelecionada = repositorioDisciplina.SelecionarPorId(disciplina.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(disciplinaSelecionada);
        Assert.AreEqual("NomeAtualizado", disciplinaSelecionada.Nome);
    }

    [TestMethod]
    public void Excluir_RemoveRegistroExistente()
    {
        Disciplina disciplina = Builder<Disciplina>
            .CreateNew()
            .With(d => d.UserId = Guid.Empty)
            .Persist();

        bool conseguiuExcluir = repositorioDisciplina.Excluir(disciplina.Id);
        dbContext.ChangeTracker.Clear();

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(repositorioDisciplina.SelecionarPorId(disciplina.Id));
    }

    [TestMethod]
    public void SelecionarTodos_CarregaRegistros()
    {
        IList<Disciplina> disciplina = Builder<Disciplina>
            .CreateListOfSize(3)
            .All()
            .With(d => d.UserId = Guid.Empty)
            .Persist();

        dbContext.ChangeTracker.Clear();

        Assert.HasCount(3, repositorioDisciplina.SelecionarTodos());
    }
}
