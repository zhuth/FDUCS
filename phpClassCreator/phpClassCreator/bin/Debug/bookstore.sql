/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50045
Source Host           : localhost:3306
Source Database       : mydb

Target Server Type    : MYSQL
Target Server Version : 50045
File Encoding         : 65001

Date: 2011-05-06 17:35:39
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `book`
-- ----------------------------
DROP TABLE IF EXISTS `book`;
CREATE TABLE `book` (
  `id` int(11) NOT NULL auto_increment,
  `isbn` varchar(45) collate utf8_unicode_ci NOT NULL,
  `title` varchar(255) collate utf8_unicode_ci NOT NULL,
  `publisher` varchar(45) collate utf8_unicode_ci NOT NULL,
  `price` decimal(10,0) NOT NULL default '0',
  `qty` int(11) NOT NULL default '1',
  `author` varchar(45) collate utf8_unicode_ci NOT NULL,
  `cost` decimal(10,0) NOT NULL default '0',
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of book
-- ----------------------------
INSERT INTO `book` VALUES ('4', '1234', '1234', '1234', '0', '0', '1234', '0');
INSERT INTO `book` VALUES ('5', '1234', '1234', '1234', '0', '0', '1234', '0');

-- ----------------------------
-- Table structure for `member`
-- ----------------------------
DROP TABLE IF EXISTS `member`;
CREATE TABLE `member` (
  `id` int(11) NOT NULL auto_increment,
  `username` varchar(45) collate utf8_unicode_ci NOT NULL,
  `password` varchar(45) collate utf8_unicode_ci NOT NULL,
  `gender` tinyint(1) NOT NULL,
  `birthday` datetime NOT NULL,
  `cardno` varchar(45) collate utf8_unicode_ci NOT NULL default '',
  `accountremain` decimal(10,0) NOT NULL,
  `expire` datetime NOT NULL,
  `status` int(11) NOT NULL default '0',
  `groupid` int(11) NOT NULL default '1',
  `point` int(11) NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `fk_member_groupid` (`groupid`),
  CONSTRAINT `fk_member_groupid` FOREIGN KEY (`groupid`) REFERENCES `membergroup` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of member
-- ----------------------------
INSERT INTO `member` VALUES ('4', 'admin', '900150983cd24fb0d6963f7d28e17f72', '1', '1990-11-06 00:00:00', '1234', '0', '0000-00-00 00:00:00', '0', '2', '0');

-- ----------------------------
-- Table structure for `membergroup`
-- ----------------------------
DROP TABLE IF EXISTS `membergroup`;
CREATE TABLE `membergroup` (
  `id` int(11) NOT NULL auto_increment,
  `name` varchar(45) collate utf8_unicode_ci NOT NULL,
  `discount` decimal(10,0) NOT NULL default '1',
  `minpoint` int(11) NOT NULL,
  `authority` int(11) NOT NULL default '1',
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of membergroup
-- ----------------------------
INSERT INTO `membergroup` VALUES ('1', '默认用户分组', '1', '0', '1');
INSERT INTO `membergroup` VALUES ('2', '管理员', '1', '0', '65535');

-- ----------------------------
-- Table structure for `order`
-- ----------------------------
DROP TABLE IF EXISTS `order`;
CREATE TABLE `order` (
  `id` int(11) NOT NULL auto_increment,
  `member_id` int(11) NOT NULL,
  `placetime` datetime NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `fk_order_member1` (`member_id`),
  CONSTRAINT `fk_order_member1` FOREIGN KEY (`member_id`) REFERENCES `member` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of order
-- ----------------------------
INSERT INTO `order` VALUES ('1', '4', '2011-05-06 17:27:09');
INSERT INTO `order` VALUES ('2', '4', '2011-05-06 17:28:11');
INSERT INTO `order` VALUES ('3', '4', '2011-05-06 17:32:36');
INSERT INTO `order` VALUES ('4', '4', '2011-05-06 17:33:24');

-- ----------------------------
-- Table structure for `transaction`
-- ----------------------------
DROP TABLE IF EXISTS `transaction`;
CREATE TABLE `transaction` (
  `id` int(11) NOT NULL auto_increment,
  `book_id` int(11) default NULL,
  `qty` int(11) NOT NULL,
  `cost` int(11) NOT NULL,
  `status` int(11) NOT NULL default '0',
  `order_id` int(11) NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `fk_transaction_book1` (`book_id`),
  KEY `fk_transaction_order1` (`order_id`),
  CONSTRAINT `fk_transaction_book1` FOREIGN KEY (`book_id`) REFERENCES `book` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_transaction_order1` FOREIGN KEY (`order_id`) REFERENCES `order` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of transaction
-- ----------------------------
INSERT INTO `transaction` VALUES ('19', '4', '-12', '0', '1', '4');
INSERT INTO `transaction` VALUES ('20', '5', '-12', '0', '1', '4');

-- ----------------------------
-- View structure for `bookview`
-- ----------------------------
DROP VIEW IF EXISTS `bookview`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `bookview` AS (select `b`.`id` AS `ID`,`b`.`isbn` AS `ISBN`,`b`.`title` AS `标题`,`b`.`publisher` AS `出版商`,`b`.`cost` AS `进货价`,`b`.`price` AS `售价`,`b`.`qty` AS `存货量`,`b`.`author` AS `作者` from `book` `B`);

-- ----------------------------
-- View structure for `membergroupview`
-- ----------------------------
DROP VIEW IF EXISTS `membergroupview`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `membergroupview` AS (select `membergroup`.`id` AS `ID`,`membergroup`.`name` AS `组名`,`membergroup`.`discount` AS `折扣`,`membergroup`.`minpoint` AS `最少需要积分`,`membergroup`.`authority` AS `权限` from `membergroup`);

-- ----------------------------
-- View structure for `memberview`
-- ----------------------------
DROP VIEW IF EXISTS `memberview`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `memberview` AS (select `m`.`id` AS `ID`,`m`.`username` AS `用户名`,(select (case when (`m`.`gender` = 0) then _utf8'女' else _utf8'男' end) AS `CASE WHEN M.gender = 0 THEN '女' ELSE '男' END`) AS `性别`,`m`.`birthday` AS `生日`,`mg`.`name` AS `所属组`,`mg`.`id` AS `组ID`,`m`.`expire` AS `有效期至`,(select (case when (`m`.`status` = 0) then _utf8'正常' when (`m`.`status` = 1) then _utf8'挂失' else _utf8'其他' end) AS `CASE WHEN M.status=0 THEN '正常' WHEN M.status=1 THEN '挂失' ELSE '其他' END`) AS `状态` from (`member` `M` join `membergroup` `MG`) where (`m`.`groupid` = `mg`.`id`));

-- ----------------------------
-- View structure for `orderview`
-- ----------------------------
DROP VIEW IF EXISTS `orderview`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `orderview` AS select `order`.`id` AS `ID`,`order`.`member_id` AS `用户ID`,`order`.`placetime` AS `操作时间`,`memberview`.`用户名` AS `用户名` from (`order` join `memberview`);

-- ----------------------------
-- View structure for `transactionview`
-- ----------------------------
DROP VIEW IF EXISTS `transactionview`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `transactionview` AS (select `t`.`id` AS `ID`,`t`.`order_id` AS `订单编号`,`t`.`qty` AS `数量`,`t`.`cost` AS `总价`,`t`.`status` AS `状态`,`b`.`ID` AS `书目ID`,`b`.`标题` AS `书目标题` from (`transaction` `T` join `bookview` `B`) where (`b`.`ID` = `t`.`book_id`));

-- ----------------------------
-- Procedure structure for `Search`
-- ----------------------------
DROP PROCEDURE IF EXISTS `Search`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `Search`(tname varchar(50), word varchar(50))
BEGIN
DECLARE cname varchar(50);
DECLARE done INTEGER DEFAULT 0;
DECLARE stmt varchar(1000);
DECLARE col CURSOR FOR SELECT COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = tname;
DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
OPEN col;

SET @st = 'CONCAT_WS('',''';
FETCH NEXT FROM col INTO cname;
WHILE done = 0 DO
    SET @st = CONCAT(@st, ',`', cname, '`');
    FETCH NEXT FROM col INTO cname;
END WHILE;

SET @st = CONCAT(@st, ')'); -- CONCAT_WS(',',...)
SET @st = CONCAT('SELECT TV.* FROM (SELECT `ID`, ', @st, ' AS TR FROM ', tname, ' TT) AS TEMP, ', tname, 'View AS TV WHERE TV.ID = TEMP.ID AND TEMP.TR LIKE ''', word, '''');
-- SELECT @st;
PREPARE stmt FROM @st;
EXECUTE stmt;
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SearchCount`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SearchCount`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SearchCount`(tname varchar(50), word varchar(50))
BEGIN
DECLARE cname varchar(50);
DECLARE done INTEGER DEFAULT 0;
DECLARE stmt varchar(1000);
DECLARE col CURSOR FOR SELECT COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = tname;
DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
OPEN col;

SET @st = 'CONCAT_WS('',''';
FETCH NEXT FROM col INTO cname;
WHILE done = 0 DO
    SET @st = CONCAT(@st, ',`', cname, '`');
    FETCH NEXT FROM col INTO cname;
END WHILE;

SET @st = CONCAT('SELECT COUNT(`ID`) FROM ', tname, ' WHERE ', @st, ') LIKE ''', word, '''');
-- SELECT @st;
PREPARE stmt FROM @st;
EXECUTE stmt;
END
;;
DELIMITER ;
