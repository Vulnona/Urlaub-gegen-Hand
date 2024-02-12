DELETE FROM skills WHERE Skill_ID>0;
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (1,'Handwerk', NULL);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (2,'Haushalt', NULL);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (3,'Holz', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (4,'Metall', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (5,'Tapezieren', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (6,'Wände streichen', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (7,'Putzen', 2);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (8,'Aufräumen', 2);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (9,'Einkaufen', 2);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (10,'Kochen', 2);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (11,'zur Hand gehen - keine speziellen Kenntnisse notwendig', NULL);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (12,'Tiersitting', NULL);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (13,'Haussitting', 11);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (14,'Pferde', 12);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (15,'Kleintiere', 12);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (16,'Vögel oder Amphibien', 12);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (17,'Gartenarbeit', 2);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (18,'Trockenbau', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (19,'Estrichleger', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (20,'Fliesen verlegen', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (21,'Installationsarbeiten Bad/Heizung/Sanitär', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (22,'Installationsarbeiten Elektro', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (23,'Zimmermann', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (24,'Tischler', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (25,'Dachdecker', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (26,'Maurer', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (27,'KFZ Reparatur', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (28,'Informatiker', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (29,'Backen', 2);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (30,'Kreativ', NULL);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (31,'Marketing', 30);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (32,'Social Media', 30);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (33,'Texter', 30);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (34,'Grafiker', 30);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (35,'Fotografen', 30);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (36,'Journalisten', 30);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (37,'Webseite - Blog schreiben', 30);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (38,'Finanzen/Buchhaltung', NULL);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (39,'Körpertherapie (wie Massage, Jin Shin Jyutsu, Fussreflex)', NULL);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (40,'Führerschein', NULL);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (41,'Führerschein PKW', 40);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (42,'Führerschein LKW', 40);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (43,'Kettensägenschein vorhanden', 1);
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`)
VALUES (44,'Brennholz hacken', 1);