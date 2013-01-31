DROP TABLE IF EXISTS `login`;
CREATE TABLE `login` (
  `idlogin` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `uname` varchar(45) NOT NULL,
  `passhash` varchar(150) NOT NULL,
  `email` varchar(150) NOT NULL,
  `access` int(10) unsigned NOT NULL DEFAULT '1',
  `lc` varchar(100) NOT NULL,
  PRIMARY KEY (`idlogin`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;