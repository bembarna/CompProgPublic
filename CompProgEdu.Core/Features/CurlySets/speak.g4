grammar speak;
/*
 * Parser Rules
 */
code_namespace : using* namespace?;
code_class : using* class?;

using : using_body;
using_body : USING using_value_predicate? SEMICOLON ;
using_value_predicate: (using_dir+) {_input.Lt(+1).Type == SEMICOLON}?;
using_dir : trash_value;

namespace : namespace_signature namespace_body ;
namespace_body : OPEN_CURLY namespace_statment_list_predicate? CLOSE_CURLY  ;
namespace_statment_list_predicate: {_input.Lt(-1).Type == OPEN_CURLY}? namespace_statment* {_input.Lt(+1).Type == CLOSE_CURLY}? ;
namespace_statment : class ;
namespace_signature : NAMESPACE namespace_dir;
 namespace_dir: namespace_dir_body+;
 namespace_dir_body
 :   ~OPEN_CURLY// match any token other than a line break zero or more times
 ;

class : class_signature class_body ;

class_body : OPEN_CURLY class_statment_list_predicate? CLOSE_CURLY  ;
class_statment_list_predicate: {_input.Lt(-1).Type == OPEN_CURLY}? class_statment* {_input.Lt(+1).Type == CLOSE_CURLY}? ;
class_statment :   statement | class;
class_signature : class_head (class_inheritance)? ;
class_head : class_head_list complex_type ;
class_head_list : 
     access_modifier? ABSTRACT? STATIC? class_type;
class_type : CLASS | STRUCT;
class_inheritance: COLON class_inheritance_body ;
class_inheritance_body: inheritance_body ;
inheritance_body:  inheritance+;
inheritance:  inheritance_instance_predicate COMMA? ;
inheritance_instance_predicate: {_input.Lt(-1).Type == COMMA || _input.Lt(-1).Type == COLON}? inheritance_instance ;
inheritance_instance: complex_type ;

statement: ((statement_head ) (goto_method? | goto_property? | goto_variable? | goto_undefined_variable?));
statement_head:
    {
	    if (_input.Lt(1).Text.Contains("."))
        {
		    line_statement();
                return _localctx;
	    }
	    else if ((_input.Lt(2).Text.Contains("(") 
        || _input.Lt(2).Text.Contains("{") 
        || _input.Lt(2).Text.Contains(";") 
        || _input.Lt(2).Text.Contains("}") 
        || _input.Lt(2).Text.Contains(")")
        || _input.Lt(2).Text.Contains("=")))
        {
		    line_statement();
                return _localctx;
	    }
    }
    (access_modifier?
    OVERRIDE?
    ABSTRACT?
    STATIC?
    ASYNC?
    READONLY?
    REFERANCE?
    complex_descriptor);

goto_method: 
{
if(_input.Lt(+1).Type != OPEN_PAREN){
    			ExitRule();
                return _localctx;
}} method ;

goto_property: 
{
if(_input.Lt(+1).Type != OPEN_CURLY){

    			ExitRule();
                return _localctx;
}} property ;

goto_variable: 
{
if(_input.Lt(+1).Type != EQUAL){
    			ExitRule();
                return _localctx;
}} variable ;

goto_undefined_variable: 
{
if(_input.Lt(+1).Type != SEMICOLON){
    			ExitRule();
                return _localctx;
}} undefined_variable_semicolon ;

line_statement: trasher2 undefined_variable_semicolon ;


method : method_param method_body;
method_body : OPEN_CURLY method_statment_list_predicate? CLOSE_CURLY ;
method_statment_list_predicate: {_input.Lt(-1).Type == OPEN_CURLY}? method_statment {_input.Lt(+1).Type == CLOSE_CURLY}?;
method_statment : statement_statement*;
method_param : OPEN_PAREN method_param_body_predicate CLOSE_PAREN;
method_param_body_predicate: {_input.Lt(-1).Type == OPEN_PAREN}? param_body {_input.Lt(+1).Type == CLOSE_PAREN}?;
param_body: param+ ;
param:  param_instance_predicate COMMA? ;
param_instance_predicate: {_input.Lt(-1).Type == COMMA || _input.Lt(-1).Type == OPEN_PAREN}? param_instance ;
param_instance: complex_descriptor;

paren_statement : paren_keyword parn_param paren_body ;
paren_body : OPEN_CURLY paren_list_predicate? CLOSE_CURLY ;
paren_list_predicate: {_input.Lt(-1).Type == OPEN_CURLY}? paren_body_statment {_input.Lt(+1).Type == CLOSE_CURLY}? ;
paren_body_statment : statement_statement* ;
parn_param : OPEN_PAREN paren_param_body_predicate? CLOSE_PAREN ;
paren_param_body_predicate: {_input.Lt(-1).Type == OPEN_PAREN}? paren_param_body {_input.Lt(-1).Type == CLOSE_PAREN}? ;
paren_param_body: paren+ ;
paren: open_paren_filler | trasher ;

non_parn_statement : non_paren_keyword non_paren_body ;
non_paren_body : OPEN_CURLY non_paren_list_predicate? CLOSE_CURLY ;
non_paren_list_predicate: {_input.Lt(-1).Type == OPEN_CURLY}? non_paren_body_statment {_input.Lt(+1).Type == CLOSE_CURLY}? ;
non_paren_body_statment : statement_statement* ;

statement_statement : statement | return_statement | paren_statement | non_parn_statement ;

trasher: trash_body+;
 trash_body
 :   ~(OPEN_PAREN|CLOSE_PAREN)// match any token other than a line break zero or more times
 ;
 trasher2: trash_body2+;
 trash_body2
 :   ~SEMICOLON// match any token other than a line break zero or more times
 ;

open_paren_filler: OPEN_PAREN woah* CLOSE_PAREN;
woah: ~(OPEN_PAREN|CLOSE_PAREN);

return_statement : RETURN (return_body | SEMICOLON) ;
return_body : return_body_predicate? SEMICOLON  ;
return_body_predicate: (return_body_value+) {_input.Lt(+1).Type == SEMICOLON}? ;
return_body_value : trash_value;

method_complex_type:  var_description | generic_type;

complex_descriptor: primitive_typed_descriptor | custom_descriptor | generic_descriptor | void_descriptor; //gene

void_descriptor: VOID var_description;

primitive_typed_descriptor: primitive_type var_description;

custom_descriptor : var_description WHITESPACE+ var_description;

generic_descriptor : generic_type WHITESPACE* var_description;

complex_type : generic_type | var_description;

generic_type : var_description generic;

generic: OPEN_CARROT generic_predicate CLOSE_CARROT ;
generic_predicate: {_input.Lt(-1).Type == OPEN_CARROT}? generic_comma {_input.Lt(+1).Type == CLOSE_CARROT}?;
generic_comma:  (genertic_attribute_predicate COMMA?)+;
genertic_attribute_predicate:{_input.Lt(-1).Type == COMMA || _input.Lt(-1).Type == OPEN_CARROT}? genertic_attribute ;
genertic_attribute: var_description generic;

property : property_body ;
property_body : OPEN_CURLY property_throw_away? CLOSE_CURLY ;
property_throw_away: {_input.Lt(-1).Type == OPEN_CURLY}? property_statment* {_input.Lt(+1).Type == CLOSE_CURLY}? ;
property_statment : trash_can ;

variable : variable_body ;
variable_body : EQUAL variable_value_predicate? SEMICOLON ;
variable_value_predicate: {_input.Lt(-1).Type == EQUAL}? (variable_value+) {_input.Lt(+1).Type == SEMICOLON}?;
variable_value : trash_value;

undefined_variable_semicolon : SEMICOLON ;

trash_can: trash_description trash_can_body ;
trash_can_body: OPEN_CURLY property_throw_away? CLOSE_CURLY ;
trash_can_statment_list_predicate: {_input.Lt(-1).Type == OPEN_CURLY}? trash_can_statment* {_input.Lt(+1).Type == CLOSE_CURLY }?;
trash_can_statment : trash_can ;

access_modifier:     
    PUBLIC
    |PRIVATE
    |PROTECTED
    |INTERNAL  
    ;

paren_keyword : 
    IF
    |ELSE_IF
    |SWITCH
    |WHILE
    |FOR
    |FOREACH
    |CATCH
;

non_paren_keyword : 
    TRY|
    ELSE
    |CATCH
;

trash_description: VAR_DESCRIPTOR WHITESPACE*;

trash_value
 : ~SEMICOLON// match any token other than a line break zero or more times
 ;

var_description: VAR_DESCRIPTOR;

primitive_type
:      
     BYTE 
    |SBYTE 
    |SHORT 
    |USHORT 
    |INT 
    |UINT 
    |LONG 
    |ULONG 
    |FLOAT 
    |DOUBLE 
    |DECIMAL 
    |CHAR 
    |BOOL 
    |STRING 
    |VAR
;

/*
 * Lexer Rules
 */
fragment A          : ('a') ;
fragment B          : ('b') ;
fragment C          : ('c') ;
fragment D          : ('d') ;
fragment E          : ('e') ;
fragment F          : ('f') ;
fragment G          : ('g') ;
fragment H          : ('h') ;
fragment I          : ('i') ;
fragment J          : ('j') ;
fragment K          : ('k') ;
fragment L          : ('l') ;
fragment M          : ('m') ;
fragment N          : ('n') ;
fragment O          : ('o') ;
fragment P          : ('p') ;
fragment Q          : ('q') ;
fragment R          : ('r') ;
fragment S          : ('s') ;
fragment T          : ('t') ;
fragment U          : ('u') ;
fragment V          : ('v') ;
fragment W          : ('w') ;
fragment X          : ('x') ;
fragment Y          : ('y') ;
fragment Z          : ('z') ;

fragment Eq          : ('=') ;

fragment Period : ('.') ;

fragment OpenPara          : ('(') ;
fragment ClosePara          : (')') ;
fragment OpenCurly          : ('{') ;
fragment CloseCurly          : ('}') ;
fragment Comma          : (',') ;
fragment SemiColon          : (';') ;
fragment Colon          : (':') ;
fragment OpenCarrot          : ('<') ;
fragment CloseCarrot          : ('>') ;
fragment OpenBracket          : ('[') ;
fragment CloseBracket          : (']') ;

fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment NUMBER : [0-9] ;
fragment UNDERSCORE : ('_');
fragment AT : ('@');

WHITESPACE : ' '+;

//Access Modifiers
PUBLIC  : WHITESPACE? P U B L I C WHITESPACE?;
PRIVATE  : WHITESPACE? P R I V A T E WHITESPACE?;
PROTECTED  :WHITESPACE? P R O T E C T E D WHITESPACE?;
INTERNAL :WHITESPACE? I N T E R N A L WHITESPACE?;

//Static
STATIC : WHITESPACE? S T A T I C WHITESPACE?;

//Void
VOID : WHITESPACE? V O I D WHITESPACE?;

//Abstract
ABSTRACT : WHITESPACE? A B S T R A C T WHITESPACE?;

//ReadOnly
READONLY : WHITESPACE? R E A D O N L Y WHITESPACE?;

//Async
ASYNC : WHITESPACE? A S Y N C WHITESPACE?;

//Referance
REFERANCE : WHITESPACE? R E F WHITESPACE?;

//Class
CLASS               : WHITESPACE? C L A S S WHITESPACE? ; 

//Struct
STRUCT               : WHITESPACE? S T R U C T WHITESPACE? ; 

//Namespace
NAMESPACE               : WHITESPACE? N A M E S P A C E WHITESPACE? ; 

//Break
BREAK               : WHITESPACE? B R E A K WHITESPACE? ; 

//Base
BASE               : WHITESPACE? B A S E WHITESPACE? ; 

//When
WHEN    : WHITESPACE? W H E N WHITESPACE?;

//Const
CONST : WHITESPACE? C O N S T WHITESPACE?;

//Using
USING : WHITESPACE? U S I N G WHITESPACE?;

//Return
RETURN : WHITESPACE? R E T U R N WHITESPACE?;

//Override
OVERRIDE : WHITESPACE? O V E R R I D E WHITESPACE?;

//Statements
IF : WHITESPACE? I F WHITESPACE?;
ELSE : E L S E WHITESPACE?;
ELSE_IF : E L S E WHITESPACE IF WHITESPACE?;
DO: D O WHITESPACE?;
WHILE: W H I L E WHITESPACE?;
FOR: F O R WHITESPACE?;
FOREACH: F O R E A C H WHITESPACE?;
TRY: T R Y WHITESPACE?;
CATCH: C A T C H WHITESPACE?;
FINALLY: F I N A L L Y WHITESPACE?;
SWITCH: S W I T C H WHITESPACE?;

//PrimitiveType
BYTE : B Y T E WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | B Y T E;
SBYTE : S B Y T E WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | S B Y T E;
SHORT : S H O R T WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | S H O R T ;
USHORT : U S H O R T WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | U S H O R T;
INT : I N T WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | I N T ;
UINT : U I N T WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | U I N T ;
LONG : L O N G WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | L O N G ;
ULONG : U L O N G WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | U L O N G;
FLOAT : F L O A T WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | F L O A T;
DOUBLE : D O U B L E WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | D O U B L E;
DECIMAL : D E C I M A L WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | D E C I M A L;
CHAR : C H A R  WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | C H A R ; 
BOOL : B O O L  WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | B O O L ;
STRING : S T R I N G  WHITESPACE? (OPEN_BRACKET WHITESPACE? CLOSE_BRACKET) | S T R I N G ;
VAR : V A R;

//SKIPME          : (' ')+ -> skip ;

//Parentheses
OPEN_PAREN : OpenPara | OpenPara WHITESPACE? | WHITESPACE? OpenPara WHITESPACE?;
CLOSE_PAREN : ClosePara | ClosePara WHITESPACE? | WHITESPACE? ClosePara WHITESPACE?;

//Comma
COMMA : Comma | Comma WHITESPACE? | WHITESPACE? Comma WHITESPACE?;

//SemiColon
SEMICOLON : SemiColon | SemiColon WHITESPACE? | WHITESPACE? SemiColon WHITESPACE?;

//Colon
COLON : Colon | Colon WHITESPACE? | WHITESPACE? Colon WHITESPACE?;

//Carrots
OPEN_CARROT : OpenCarrot | OpenCarrot WHITESPACE? | WHITESPACE? OpenCarrot WHITESPACE?;
CLOSE_CARROT : CloseCarrot| CloseCarrot WHITESPACE? | WHITESPACE? CloseCarrot WHITESPACE?;

//Brackets
OPEN_BRACKET : OpenBracket| OpenBracket WHITESPACE? | WHITESPACE? OpenBracket WHITESPACE?;
CLOSE_BRACKET : CloseBracket| CloseBracket WHITESPACE? | WHITESPACE? CloseBracket WHITESPACE?;

//CloseCurly
OPEN_CURLY : OpenCurly| OpenCurly WHITESPACE? | WHITESPACE? OpenCurly WHITESPACE?;
CLOSE_CURLY  : CloseCurly| CloseCurly WHITESPACE? | WHITESPACE? CloseCurly WHITESPACE?;

EQUAL :  Eq | Eq WHITESPACE? | WHITESPACE? Eq WHITESPACE?;

STATEMENT:
(
    IF
    |ELSE
    |DO
    |WHILE
    |FOR
    |FOREACH
    |TRY
    |CATCH
    |FINALLY
);

VAR_DESCRIPTOR  : (LOWERCASE | UPPERCASE | NUMBER | Period | UNDERSCORE | AT | OpenBracket | CloseBracket)+;