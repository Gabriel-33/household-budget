using Microsoft.EntityFrameworkCore;
using HouseHoldeBudgetApi.Context;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Enums;

namespace HouseHoldeBudgetApi.Repositories;

/// <summary>
/// Implementação genéricas do repositório <see cref="IUsuarioRepository"/>.
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public UsuarioRepository(AppDbContext dbContext, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<UsuarioModel?> GetUsuarioById(int id, bool onlyFindAsync)
    {
        UsuarioModel? usuarioModel = null;

       usuarioModel = await _dbContext.usuarios.FindAsync(id);
       
        if (usuarioModel is null) return null;

        return usuarioModel;
    }

    public async Task<UsuarioModel?> GetUsuarioByEmail(string email, bool isUserActive = false)
    {
        UsuarioModel? userModel = null;

        
        userModel = await _dbContext.usuarios
        .FirstOrDefaultAsync(u => u.emailUsuario == email);

        if (userModel is null) return null;

        return userModel;
    }

    public Task<IList<UsuarioModel>> GetUsers(int page, int pageSize) =>
        GetUsers(_dbContext, page, pageSize);

    private async Task<IList<UsuarioModel>> GetUsersWFactory(int page, int pageSize,int userType = 0, int statusUsuario = 0, bool onlyProfessor = false)
    {
        await using AppDbContext? inDbContext = await _dbContextFactory.CreateDbContextAsync();

        if (inDbContext is null)
            throw new("Was not possible to instanciaite a new DbContext");

        return await GetUsers(inDbContext, page, pageSize,userType,statusUsuario, onlyProfessor);
    }

    private async Task<IList<UsuarioModel>> GetUsers(AppDbContext inDbContext, int page, int pageSize,int userType = 0, int statusUsuario = 0, bool onlyProfessor = false)
    {
        var result = new List<UsuarioModel>();
        
        var query = inDbContext.usuarios
            .AsNoTracking()
            .OrderBy(f => f.idUsuario)
            .AsQueryable();
        
        // paginação e execução da query
        result = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return result;

    }

    public Task<int> GetUsersCount() =>
        GetUsersCount(_dbContext);

    private async Task<int> GetUsersCountWFactory(int userType, int statusUsuario)
    {
        await using AppDbContext? inDbContext = await _dbContextFactory.CreateDbContextAsync();

        if (inDbContext is null)
            throw new("Was not possible to instanciaite a new DbContext");

        return await GetUsersCount(inDbContext,userType, statusUsuario);
    }

    private async Task<int> GetUsersCount(AppDbContext inDbContext, int userType = 0, int statusUsuario = 0)
    {
        var query = inDbContext.usuarios.AsQueryable();
        
        return await query.CountAsync();
    }


    public async Task<(IList<UsuarioModel>, int, int)> GetUsersAndCount(int page, int pageSize,int userType = 0, int statusUsuario = 0, bool onlyProfessor = false)
    {
        var usersTask = GetUsersWFactory(page, pageSize, userType,statusUsuario, onlyProfessor);
        var usersCountTask = GetUsersCountWFactory(userType, statusUsuario);
        await Task.WhenAll(usersTask, usersCountTask);

        var result = usersTask.Result;
        int usersCount = usersCountTask.Result;
        return (result, result.Count, usersCount);
    }

    public async Task<bool> CheckUserByMatriculaAndEmail(string matricula, string email)
    {
        bool exists = await _dbContext.usuarios
            .AnyAsync(u => u.emailUsuario == email);
        return exists;
    }

    public async Task CreateUser(UsuarioModel usuarioModel) =>
        await _dbContext.usuarios.AddAsync(usuarioModel);

    public void DeleteUser(UsuarioModel usuario) =>
        _dbContext.usuarios.Remove(usuario);

    public async Task FlushChanges() =>
        await _dbContext.SaveChangesAsync();
}