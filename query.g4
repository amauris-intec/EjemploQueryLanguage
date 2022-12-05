grammar query;

query: join (COR1 condicion COR2)? (PUNTO seleccion)?  #queryConJoin
     | tabla (COR1 condicion COR2)? (PUNTO seleccion)? #querySinJoin
     ;
     
join : tabla1=tabla tipo=(AND|LT|GT) tabla2=tabla (LLAVE1 condicion LLAVE2)?
     ;

condicion : criterio                  #condicionUnica
          | criterio (COMA criterio)+ #condicionMultiple
          ;

criterio : (alias=ID PUNTO)? campo=ID IGUAL NUM                      #criterioNumerico
         | (alias=ID PUNTO)? campo=ID IGUAL TEXTO                    #criterioAlphanumerico
         | alias1=ID PUNTO campo1=ID IGUAL alias2=ID PUNTO campo2=ID #criterioJoin
         ;
         
         
tabla: nombre=ID                    #tablaSinAlias
     | nombre=ID PAR1 alias=ID PAR2 #tablaConAlias
     ;

seleccion : ID            #seleccionUnica
          | ID (COMA ID)+ #seleccionMultiple
          ;

LLAVE1 : '{' ;
LLAVE2 : '}' ;
LT: '<' ;
GT: '>' ;
AND : '&' ;
IGUAL : '=' ;
COR1 : '[' ;
COR2 : ']' ;
PAR1 : '(' ;
PAR2 : ')' ;
COMILLAS : '"';
COMA : ',' ;
PUNTO : '.' ;
NUM : [0-9]+ ;
TEXTO : COMILLAS ~'"'* COMILLAS ;
ID : [A-Za-z_] [A-Za-z0-9_]* ;

WS : [\t ]+ -> skip ;