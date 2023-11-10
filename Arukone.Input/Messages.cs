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
            
            {0} Spielfelder in der Größe {1}*{1} werden generiert...
            Dieser Vorgang kann einen Augenblick dauern.

            """;

        internal const string FinishedBoardGeneration = """
            
                  `.                      `..                                  
                 `. ..                    `..                                  
                `.  `..    `. `...`..  `..`..  `..   `..    `.. `..     `..    
               `..   `..    `..   `..  `..`.. `..  `..  `..  `..  `.. `.   `.. 
              `...... `..   `..   `..  `..`.`..   `..    `.. `..  `..`..... `..
             `..       `..  `..   `..  `..`.. `..  `..  `..  `..  `..`.        
            `..         `..`...     `..`..`..  `..   `..    `...  `..  `....   
            
            Es wurden {0} Spielfelder mit einer Größe von {1}*{1} erstellt.
            Die Ergebnisse und Logs kannst du unter "{2}" bzw. "{3}" einsehen.

            | Der Vorgang hat {4}s gedauert.
            """;
    }
}
