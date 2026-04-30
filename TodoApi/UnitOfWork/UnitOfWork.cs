using TodoApi.Context;
using TodoApi.Repositories.Auth;
using TodoApi.Repositories.TodoItems;
using TodoApi.Repositories.TodoLists;

namespace TodoApi.UOF
{
        public class UnitOfWork
        {
            private readonly CommandDbContext _context;
            private bool _disposed = false;
            public IWritableTodoItemRepository TodoItemRepo;
            public IWritableTodoListRepository TodoListRepo;
            public IRefreshTokenRepository RefreshTokenRepo;
            public UnitOfWork(CommandDbContext context,
                IWritableTodoItemRepository TodoItemRepo,
                IWritableTodoListRepository TodoListRepo,
                IRefreshTokenRepository RefreshTokenRepo)
            {
                _context = context;
                this.TodoItemRepo = TodoItemRepo;
                this.TodoListRepo = TodoListRepo;
                this.RefreshTokenRepo = RefreshTokenRepo;
            }

            public async Task<int> CompleteAsync()
            {
                return await _context.SaveChangesAsync();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _context.Dispose();
                    }
                }
                _disposed = true;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

        }
}
