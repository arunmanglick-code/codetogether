CREATE TABLE `LookUpTagLog` (
  `idTagLookUp` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(25) DEFAULT NULL,
  `LastName` varchar(25) DEFAULT NULL,
  `Email` varchar(45) DEFAULT NULL,
  `Phone` varchar(15) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `TagName` varchar(45) DEFAULT NULL,
  `TagEnrollId` int(11) DEFAULT NULL,
  `TagFound` varchar(5) DEFAULT NULL,
  `SentEmailToSubscriber` varchar(5) DEFAULT 'No',
  `SentEnvelopToGoodJohn` varchar(5) DEFAULT 'No',
  `RecievedDeviceFromGoodJohn` varchar(5) DEFAULT 'No',
  `SentGiftCardToGoodJohn` varchar(5) DEFAULT 'No',
  `SentDeviceShipmentToSubscriber` varchar(5) DEFAULT 'No',
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`idTagLookUp`),
  KEY `TagEnrollForeignKey_idx` (`TagEnrollId`),
  CONSTRAINT `TagEnrollForeignKey` FOREIGN KEY (`TagEnrollId`) REFERENCES `TagEnrollNew` (`idTagEnroll`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `TagEnrollNew` (
  `idTagEnroll` int(11) NOT NULL AUTO_INCREMENT,
  `MDN` varchar(15) NOT NULL,
  `FirstName` varchar(25) DEFAULT NULL,
  `LastName` varchar(25) DEFAULT NULL,
  `Email` varchar(45) DEFAULT NULL,
  `TagName` varchar(45) DEFAULT NULL,
  `TagDescription` varchar(65) DEFAULT NULL,
  `Status` varchar(15) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ActivationDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`idTagEnroll`)
) ENGINE=InnoDB AUTO_INCREMENT=369 DEFAULT CHARSET=latin1;

DELIMITER $$
CREATE DEFINER=`nodesqsdb`@`%` PROCEDURE `sp_get_TagEnrollment`(in TagName1 varchar(45))
BEGIN
  SELECT * FROM TagEnrollNew
    WHERE `TagName` = TagName1 AND `Status` = 'Active';
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`nodesqsdb`@`%` PROCEDURE `sp_get_TagLookupandTagEnroll`(
in TagName1 varchar(25)
)
BEGIN
SELECT LTL.FirstName as GJ_FirstName,LTL.LastName as GJ_LastName, LTL.Email as GJ_Email,LTL.Phone as GJ_Phone,LTL.Address as GJ_Address,LTL.CreatedDate as GJ_LookUpDate,
TEN.FirstName as Sub_FirstName,TEN.LastName as Sub_LastName,TEN.Email as Sub_Email,TEN.MDN,LTL.TagName,LTL.TagEnrollId
FROM LookUpTagLog LTL
INNER JOIN TagEnrollNew TEN
ON LTL.TagEnrollId=TEN.idTagEnroll
where 
LTL.TagName=TagName1;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`nodesqsdb`@`%` PROCEDURE `sp_insert_TagEnrollmentNew`(
  in MDN1 varchar(15),
  in FirstName1 varchar(25),
  in LastName1 varchar(25), 
  in Email varchar(45),
  in TagName1 varchar(45),
  in TagDescription1 varchar(65),
  in Status1 varchar(15)
)
BEGIN
INSERT INTO TagEnrollNew
(
`MDN`,
`FirstName`,
`LastName`,
`Email`,
`TagName`,
`TagDescription`,
`Status`,
`CreatedDate`
)
VALUES
(MDN1,FirstName1,LastName1,Email,TagName1,TagDescription1,Status1,NOW());
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`nodesqsdb`@`%` PROCEDURE `sp_insert_TAGLookUpLog`(
 in FirstName varchar(45),
 in LastName varchar(45),
 in Email varchar(45),
 in Phone varchar(45),
 in Address varchar(255),
 in TagName varchar(45),
 in TagEnrollId int(11),
 in TagFound varchar(45)
)
BEGIN
  INSERT INTO `nodesqsdb`.`LookUpTagLog`
  (`FirstName`,`LastName`,`Email`,`Phone`,`Address`,`TagName`,`TagEnrollId`,`TagFound`,`CreatedDate`)
  VALUES
  (FirstName,LastName,Email,Phone,Address,TagName,TagEnrollId,TagFound,NOW());
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`nodesqsdb`@`%` PROCEDURE `sp_reset_TagDatabase`()
BEGIN

UPDATE TagEnrollNew
SET Status = 'InActive';

DELETE FROM nodesqsdb.LookUpTagLog;

END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`nodesqsdb`@`%` PROCEDURE `sp_update_TagEnrollment`(
  in MDN_1 varchar(45),
  in TagName_1 varchar(45),
  in TagName_2 varchar(45),
  in TagName_3 varchar(45),
  in Status_1 varchar(45)
)
BEGIN

  UPDATE `TagEnrollNew`
  SET status = 'Inactive'
  WHERE `MDN` = MDN_1;

  UPDATE `TagEnrollNew`
  SET
  `Status` = Status_1,
  `ModifiedDate` = Now(),
  `ActivationDate` = if (Status_1 ='Active',Now(),NULL)
  WHERE
  `TagName` = TagName_1 and `MDN` = MDN_1;
  
   UPDATE `TagEnrollNew`
  SET
  `Status` = Status_1,
  `ModifiedDate` = Now(),
  `ActivationDate` = if (Status_1 ='Active',Now(),NULL)
  WHERE
  `TagName` = TagName_2 and `MDN` = MDN_1;
  
   UPDATE `TagEnrollNew`
  SET
  `Status` = Status_1,
  `ModifiedDate` = Now(),
  `ActivationDate` = if (Status_1 ='Active',Now(),NULL)
  WHERE
  `TagName` = TagName_3 and `MDN` = MDN_1;
  
  SELECT * FROM nodesqsdb.TagEnrollNew
  where status ='Active' and `MDN` = MDN_1;

  
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`nodesqsdb`@`%` PROCEDURE `sp_update_TAGLookUpLog`(
 in TagEnrollId_1 int(11),
 in TagName_1 varchar(45),
 in SentEmailToSubscriber_1 varchar(5),
 in SentEnvelopToGoodJohn_1 varchar(5),
 in RecievedDeviceFromGoodJohn_1 varchar(5),
 in SentGiftCardToGoodJohn_1 varchar(5),
 in SentDeviceShipmentToSubscriber_1 varchar(5)
)
BEGIN    
     UPDATE `LookUpTagLog`
    SET
    `SentEmailToSubscriber` = SentEmailToSubscriber_1,
      `SentEnvelopToGoodJohn` = SentEnvelopToGoodJohn_1,
      `RecievedDeviceFromGoodJohn` = RecievedDeviceFromGoodJohn_1,
      `SentGiftCardToGoodJohn` = RecievedDeviceFromGoodJohn_1,
      `SentDeviceShipmentToSubscriber` = SentDeviceShipmentToSubscriber_1,
    `ModifiedDate` = Now()
    WHERE
    `TagName` = TagName_1 and 
      `TagEnrollId` = TagEnrollId_1;
    
END$$
DELIMITER ;
