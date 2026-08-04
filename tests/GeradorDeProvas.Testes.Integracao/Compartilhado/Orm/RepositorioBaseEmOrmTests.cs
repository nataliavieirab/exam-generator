using GeradorDeProvas.Infra.Compartilhado.Orm;
using GeradorDeProvas.Testes.Integracao.Compartilhado.Identity;
using Microsoft.EntityFrameworkCore;

namespace GeradorDeProvas.Testes.Integracao.Compartilhado.Orm;

public abstract class RepositorioBaseEmOrmTests
{
    protected GeradorDeProvasDbContext dbContext = null!;

    [TestInitialize]
    public void InicializarContexto()
    {
        dbContext = CriarDbContext(Guid.NewGuid());
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
