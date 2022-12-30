namespace WebFayre.Models
{
    public class FairCurrentUsers
    {

        // current users inside a given fair
        public Dictionary<int, string> users { get; set; } = new Dictionary<int, string>();

        // total number of users that can attend a fair at the same time
        public int totalConcurrentUsers { get; set; }

        public FairCurrentUsers()
        {
            users = new Dictionary<int, string>();
            totalConcurrentUsers = 0;
        }
        public FairCurrentUsers(int concurrentCapacity)
        {
            users = new Dictionary<int, string>();
            if (concurrentCapacity >= 0)
                totalConcurrentUsers = concurrentCapacity;
            else throw new ArgumentException("negative number for capacity", "concurrentCapacity");
        }

        public int addUser(int userId, string userEmail)
        {

            // se ainda houver espaço para mais clientes
            if (canEnter())
            {
                // se o cliente ainda não estiver registado
                if (!userExists(userId))
                {
                    this.users.Add(userId, userEmail);
                }
                // se já estiver, nao faz nada
                else return 0;
            }

            // se não houver espaço para mais clientes
            return 1;
        }

        public int removeUser(int userId)
        {
            // numero atual de utilizadores na feira
            int currentUsers = this.users.Count;

            // se houver utilizadores na feira
            if (currentUsers > 0)
            {
                // remover utilizador, caso exista
                if (this.users.Remove(userId) == true)
                    return 0;

                // se nao existir
                else return 1;
            }
            // se a feira já estava vazia
            return 2;
        }


        public bool canEnter()
        {
            // se o número de utilizadores na feira é menor que o número total de utilizadores
            // permitidos em simultaneo
            return this.users.Count < this.totalConcurrentUsers;
        }

        public bool userExists(int userId)
        {
            return this.users.ContainsKey(userId);
        }

        public int onlineUsers()
        {
            return this.users.Count;
        }

    }
}
