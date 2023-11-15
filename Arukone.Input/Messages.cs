namespace Arukone.Input
{
    internal static class Messages
    {
        internal const string SpecifyBoardSize = """

                  `.                      `..                                  
                 `. ..                    `..                                  
                `.  `..    `. `...`..  `..`..  `..   `..    `.. `..     `..    
               `..   `..    `..   `..  `..`.. `..  `..  `..  `..  `.. `.   `.. 
              `...... `..   `..   `..  `..`.`..   `..    `.. `..  `..`..... `..
             `..       `..  `..   `..  `..`.. `..  `..  `..  `..  `..`.        
            `..         `..`...     `..`..`..  `..   `..    `...  `..  `....   
            

            Bestimme die Größe des Spielfeldes (>= {0}): 
            """;

        internal const string StartedBoardGeneration = """
            
                  `.                      `..                                  
                 `. ..                    `..                                  
                `.  `..    `. `...`..  `..`..  `..   `..    `.. `..     `..    
               `..   `..    `..   `..  `..`.. `..  `..  `..  `..  `.. `.   `.. 
              `...... `..   `..   `..  `..`.`..   `..    `.. `..  `..`..... `..
             `..       `..  `..   `..  `..`.. `..  `..  `..  `..  `..`.        
            `..         `..`...     `..`..`..  `..   `..    `...  `..  `....   
            

            Es werden {0} Spielfelder in der Größe {1}*{1} generiert.
            Dieser Vorgang kann einen Moment dauern.

            """;

        internal const string FinishedBoardGeneration = """
            
                  `.                      `..                                  
                 `. ..                    `..                                  
                `.  `..    `. `...`..  `..`..  `..   `..    `.. `..     `..    
               `..   `..    `..   `..  `..`.. `..  `..  `..  `..  `.. `.   `.. 
              `...... `..   `..   `..  `..`.`..   `..    `.. `..  `..`..... `..
             `..       `..  `..   `..  `..`.. `..  `..  `..  `..  `..`.        
            `..         `..`...     `..`..`..  `..   `..    `...  `..  `....   
            

            {0} Spielfelder mit einer Größe von {1}*{1} wurden erstellt.
            Der Vorgang wurde nach {2}s abgeschlossen.

            - Ergebnisse: {3}
            - Logs:       {4}

            Drücke eine beliebige Taste, um das Fenster zu schließen...
            """;

        internal const string ErrorOpeningExplorer = "Beim Öffnen des Explorers ist ein Fehler aufgetreten.";
    }
}
