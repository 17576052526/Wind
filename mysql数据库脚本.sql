CREATE SCHEMA `wind` ;

CREATE TABLE `wind`.`sys_admin` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_name` VARCHAR(45) NOT NULL,
  `user_pwd` VARCHAR(200) NULL,
  PRIMARY KEY (`id`));

insert into wind.sys_admin(user_name,user_pwd) values('admin','czDFiLmmkyNRDYxo3Xubrg==');

CREATE TABLE `wind`.`test_type` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `type_id` VARCHAR(45) NULL,
  `name` VARCHAR(45) NULL,
  PRIMARY KEY (`id`));

CREATE TABLE `wind`.`test_main` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `main_id` VARCHAR(45) NULL,
  `main_name` VARCHAR(45) NULL,
  `test_type_id` INT NULL,
  `quantity` INT NULL,
  `amount` DECIMAL(14,2) NULL,
  `is_show` TINYINT(1) NULL,
  `img` VARCHAR(200) NULL,
  `files` VARCHAR(200) NULL,
  `remark` VARCHAR(500) NULL,
  `create_time` DATETIME NULL,
  PRIMARY KEY (`id`));
