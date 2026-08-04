using FizzWare.NBuilder;
using GeradorDeProvas.Dominio.Modulos.ModuloDisciplina;
using GeradorDeProvas.Infra.Compartilhado.Orm;
using GeradorDeProvas.Infra.Modulos.ModuloDisciplina;
using GeradorDeProvas.Infra.Modulos.ModuloMateria;
using GeradorDeProvas.Infra.Modulos.ModuloProva;
using GeradorDeProvas.Testes.Integracao.Compartilhado.Identity;
using Microsoft.EntityFrameworkCore;
namespace GeradorDeProvas.Testes.Integracao.Compartilhado.Orm;

public abstract class RepositorioBaseEmOrmTests
{
    protected GeradorDeProvasDbContext dbContext = null!;
    protected RepositorioDisciplinaEmOrm repositorioDisciplina = null!;
    protected RepositorioProvaEmOrm repositorioProva = null!;

    [TestInitialize]
    public void InicializarContexto()
    {
        dbContext = CriarDbContext(Guid.NewGuid());

        repositorioDisciplina = new RepositorioDisciplinaEmOrm(dbContext);
        repositorioProva = new RepositorioProvaEmOrm(dbContext);

        BuilderSetup.SetCreatePersistenceMethod<Disciplina>((disciplina) =>
        {
            repositorioDisciplina.Cadastrar(disciplina);
            dbContext.ChangeTracker.Clear();
        });

        BuilderSetup.SetCreatePersistenceMethod<IList<Disciplina>>((disciplinas) =>
        {
            foreach (Disciplina d in disciplinas)
            {
                repositorioDisciplina.Cadastrar(d);
                dbContext.ChangeTracker.Clear();
            }
        });
    }

    [TestCleanup]
    public void DescartarContexto()
    {
        dbContext.Dispose();
    }

    private static GeradorDeProvasDbContext CriarDbContext(Guid userId)
    {
        DbContextOptions<GeradorDeProvasDbContext> options =
            new DbContextOptionsBuilder<GeradorDeProvasDbContext>()
                .UseInMemoryDatabase("GeradorDeProvasTestDB_Memory")
                .Options;

        return new GeradorDeProvasDbContext(options, new ProvedorDeUsuarioFake(userId));
    }
}
