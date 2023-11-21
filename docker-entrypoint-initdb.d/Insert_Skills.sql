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