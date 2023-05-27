using Tree.Entities;

namespace Tree
{
    public class TreeSeeder
    {
        private readonly TreeDbContext _dbContext;
        public TreeSeeder(TreeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Registers.Any())
                {
                    var registers = GetRegisters();
                    _dbContext.Registers.AddRange(registers);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Register> GetRegisters()
        {
            var registers = new List<Register>()
            {
                new Register()
                {
                    Path = "folder1",
                    Name = "Plik1",
                    Format = "txt",
                },
                new Register()
                {
                    Path = "folder1\\folder2",
                    Name = "Plik2",
                    Format = "txt"
                },
                new Register()
                {
                    Path = "obrazy",
                    Name = "obraz1",
                    Format = "png"
                },
            };

            return registers;
        }
        
    }
}
