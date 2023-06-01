using System.Linq;
using Tree.Entities;

namespace Tree.Models
{
    public class MappingToFolderStructure
    {
        List<FolderDto> FolderList = new List<FolderDto>();//utworzenie listy folderów które zwrócimy
        List<FileDto> FileList = new List<FileDto>();//utworzenie listy plików

        //funkcja zmieniająca dane na strukturę hierarchiczną według folderów
        public IEnumerable<FolderDto> GetFolderStructure(List<Register> registers)
        {
            // 1. dodanie folderu root do listy folderów
            FolderList.Add(new FolderDto() { Id = 0, Name = "root", SubFolders = null, Files = null, Path = null });//utworzenie folderu root
            FolderList[0].SubFolders = new List<FolderDto>();

            // 2. Weź z rejestru pierwszą ścieżkę i rozdziel na foldery
            //int folderIterInPath = 0;
            string? firstPath = registers[0].Path; // Pobieranie pierwszej ścieżki
            if (!string.IsNullOrEmpty(firstPath))
            {
                string[] folders = firstPath.Split('/');//rozdzielenie

                //utwórz wszystkie foldery w ścieżce i jeżeli w rejestrze jest plik w to go dodaj
                MakeFoldersAndFile(folders, registers,1);
                
                

            }

            // 3. Weź z rejestru kolejną ścieżkę i rozdziel na foldery
            TakeNextPath(registers, 1);
            void TakeNextPath(List<Register> registers, int numberOfRegister)
            {
                string? nextPath = registers[numberOfRegister].Path; // Pobieranie kolejnej ścieżki
                if (!string.IsNullOrEmpty(nextPath))
                {
                    string[] folders = nextPath.Split('\\');//rozdzielenie

                    PathIterations(folders, 0);
                    void PathIterations(string[] folders, int folderIterInPath)
                    {
                        // Sprawdź dla pierwszego folderu ze ścieżki czy już jest o takim samym Id lub jeśli nie ma zapisanego Id takiej samej nazwie w folderze root
                        if(FolderList[0].SubFolders != null)
                        {
                            if (FolderList[0].SubFolders.Exists(f => f.Name == folders[0]))
                            {
                                // jeśli jest to zapisz jego Id i weź kolejny folder ze ścieżki(folderListIterator) i sprawdź czy jest w folderze o zapisanym Id(savedId) folder o takiej samej nazwie
                                var id = FolderList[folderIterInPath].SubFolders.Find(f => f.Name == folders[0]).Id;
                                CheckingFolders(1, id, folders, false, registers, numberOfRegister);
                            }
                            else
                            {
                                //utwórz wszystkie foldery w ścieżce i jeżeli rejestrze jest plik w to go dodaj
                                MakeFoldersAndFile(folders, registers, numberOfRegister);
                            }
                        
                        }
                        else // jeśli root nie ma podfolderów to 
                        {
                            //utwórz wszystkie foldery w ścieżce i jeżeli rejestrze jest plik w to go dodaj
                            MakeFoldersAndFile(folders, registers, numberOfRegister);
                        }
                    }
                }
                // wróć do pkt 3. - Weź kolejną ścieżkę i powtóż działanie, do póki się nie skończą ścieżki 
                if (registers.Count > numberOfRegister+1)
                {
                    TakeNextPath(registers, numberOfRegister + 1);
                }
            }
            // Jak się skończą ścieżki to zakończ działanie i zwróć liste folderów
            return FolderList;
        }


        ////////////////        FUNKCJE POMOCNICZE             ////////////////////////

        
        void CheckingFolders(int folderListIterator, int savedId, string[] folders, bool flag, List<Register> registers, int numberOfRegister)
        {
            if (flag = true)
            {
                goto AddFolder;
            }
            // sprawdź czy jest w folderze o zapisanym Id(savedId) folder o takiej samej nazwie
            int savedIdfolderIndex = FolderList.FindIndex(f => f.Id == savedId);
            if (FolderList[savedIdfolderIndex].SubFolders.Exists(f => f.Name == folders[folderListIterator]))
            {
                // jeśli jest to zapisz jego Id i weź kolejny folder ze ścieżki(folderListIterator)
                var id = FolderList[savedIdfolderIndex].SubFolders.Find(f => f.Name == folders[folderListIterator]).Id;
                if (FolderList.Count > folderListIterator + 2)
                {
                    CheckingFolders(folderListIterator + 1, id, folders, false, registers, numberOfRegister);
                }
                // jeżeli ścieżka się skończyła to sprawdź czy do niej nie jest przypisany plik. Jeśli tak to go utwórz
                CheckIfThereIsFileIfYesMakeIt(registers, numberOfRegister);
                // Zrekonstruj ścieżkę, przypisz subfoldery
                //AssignSubfolders(registers, numberOfRegister);
            }
            else
            {
                goto AddFolder;
            }
        AddFolder:
            {
                // jeśli nie ma to utwórz obiekt folder i od razu bez sprawdzania utwórz kolejne foldery aż się skończy ścieżka sprawdź czy do ścieżki nie jest przypisany plik. Jeśli tak to go utwórz
                MakeFoldersAndFile(folders, registers,numberOfRegister);               
            }
        }


        void MakeFoldersAndFile(string[] folders, List<Register> registers,int numberOfRegister,int numberOfFolderInFolderListYouWantStart=0)
        {
            //utworzenie wszystkich folderów w ścieżce
            FolderDto FolderObject = MakeFoldersAndAddToFolderList(folders, numberOfFolderInFolderListYouWantStart);
            FolderObject.Files = new List<FileDto>();

            // jeżeli rejestrze jest plik w to go dodaj i przypisz do ostatniego folderu w ścieżce 
            if (registers[numberOfRegister].Name != null)
            {
                int id = GiveIdFile();
                FolderObject.Files.Add(new FileDto() { Id = id, Name = registers[numberOfRegister].Name, Format = registers[0].Format });
                FileList.Add(FolderObject.Files.Find(f => f.Id == id));
            }
        }
        FolderDto? MakeFoldersAndAddToFolderList(string[] folders, int iterator)
        {
            
            if (folders.Length > iterator)
            {
                //Array.Reverse(folders);
                int id = GiveIdFolder();
                FolderList.Add(new FolderDto() { Id = id, Name = folders[iterator], SubFolders = new List<FolderDto>() { MakeFoldersAndAddToFolderList(folders, iterator + 1)}, Files = null, Path = null });//utworzenie obiektu FolderDto i dodanie go do listy folderów
                //dodanie do foderu root pierwszego folderu z listy
                if (iterator == 0)
                {
                    FolderList[0].SubFolders.Add(FolderList.Find(f => f.Id == id));
                }
                
            return FolderList[id];
                
            }
            return null;//FolderList[id];
        }

        void CheckIfThereIsFileIfYesMakeIt(List<Register> registers, int numberOfRegister)
        {
            if (registers[numberOfRegister].Name != null)
            {
                FileList.Add(new FileDto() { Id = GiveIdFile(), Name = registers[numberOfRegister].Name, Format = registers[numberOfRegister].Format });
            }
        }

        int GiveIdFolder()
        {
            return GiveId2Folder(FolderList, 0);

            int GiveId2Folder(List<FolderDto> FolderList, int id)
            {
                if (FolderList.Exists(x => x.Id == id))
                {
                    return GiveId2Folder(FolderList, id + 1);
                }
                return id;

            }
        }
        

        int GiveIdFile()
        {
            return GiveId2File(FileList, 0);

            int GiveId2File(List<FileDto> FileList, int id)
            {
                if (FileList.Exists(x => x.Id == id))
                {
                    return GiveId2File(FileList, id + 1);
                }
                return id;
            }
        }
        


    }
}
