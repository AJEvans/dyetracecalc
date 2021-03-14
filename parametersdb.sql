/*
** This file builds the initial database needed. To do so, run:
** sqlite3 parametersdb.db -init parametersdb.sql
** from this directory at the terminal or a command prompt. 
** You will need to have installed SQLite:
** https://www.sqlite.org/index.html
** and added it to your path. 
** Author: AJEvans
*/

DROP TABLE IF EXISTS "Parameters";

CREATE TABLE "Parameters" (
	"PrimaryKey" INTEGER PRIMARY KEY,
	"Increment" DECIMAL NULL,
	"Tolerance" DECIMAL NULL,
	"TimeOne" INTEGER NULL,    
	"TimeTwo" INTEGER NULL,      
	"Distance" DECIMAL NULL,
	"Time" TEXT NULL,  
	"Dispersion" TEXT NULL
);


INSERT INTO "Parameters" VALUES('1','0.01','0.01','1686','1964','636.65','Peak time appears here.','Dispersion appears here.');
