<?php

class Book {

	var $id = -1, $isbn = "", $title = "", $publisher = "", $price = 0, $qty = 0, $author = "", $cost = 0;

	public function fromRow(Array $row) {
		$this->id = !isset($row['id']) ? $this->id : $row['id'];
		$this->isbn = !isset($row['isbn']) ? $this->isbn : $row['isbn'];
		$this->title = !isset($row['title']) ? $this->title : $row['title'];
		$this->publisher = !isset($row['publisher']) ? $this->publisher : $row['publisher'];
		$this->price = !isset($row['price']) ? $this->price : $row['price'];
		$this->qty = !isset($row['qty']) ? $this->qty : $row['qty'];
		$this->author = !isset($row['author']) ? $this->author : $row['author'];
		$this->cost = !isset($row['cost']) ? $this->cost : $row['cost'];
	}

	public function delete(Database $db) {
		$db->query("DELETE FROM `book` WHERE `id`=$this->id");
	}

	public function update(Database $db) {
		$t_isbn = mysql_escape_string("$this->isbn");
		$t_title = mysql_escape_string("$this->title");
		$t_publisher = mysql_escape_string("$this->publisher");
		$t_price = mysql_escape_string("$this->price");
		$t_qty = mysql_escape_string("$this->qty");
		$t_author = mysql_escape_string("$this->author");
		$t_cost = mysql_escape_string("$this->cost");
		$SQLINSERT = "INSERT INTO `book` (`isbn`, `title`, `publisher`, `price`, `qty`, `author`, `cost`) VALUES ('$t_isbn', '$t_title', '$t_publisher', $t_price, $t_qty, '$t_author', $t_cost)";
		$SQLUPDATE = "UPDATE `book` SET `id` = $this->id, `isbn` = '$t_isbn', `title` = '$t_title', `publisher` = '$t_publisher', `price` = $t_price, `qty` = $t_qty, `author` = '$t_author', `cost` = $t_cost WHERE `id` = $this->id";
		if ($this->id <= 0) $db->query($SQLINSERT); else $db->query($SQLUPDATE);
	}

	public static function fetchRecords($db, $condition = "", $recPerPage = 10, $page = 0) {
		$arr = Array();
		$result = $db->fetch('book', $condition, $recPerPage, $page * $recPerPage);
		while ($row = $db->fetchRow($result)) {
			$tmp = new book();
			$tmp->fromRow($row);
			$arr[] = $tmp;
		}
		return $arr;
	}

}

class Membergroup {

	var $id = -1, $name = "", $discount = 0, $minpoint = 0, $authority = 0;

	public function fromRow(Array $row) {
		$this->id = !isset($row['id']) ? $this->id : $row['id'];
		$this->name = !isset($row['name']) ? $this->name : $row['name'];
		$this->discount = !isset($row['discount']) ? $this->discount : $row['discount'];
		$this->minpoint = !isset($row['minpoint']) ? $this->minpoint : $row['minpoint'];
		$this->authority = !isset($row['authority']) ? $this->authority : $row['authority'];
	}

	public function delete(Database $db) {
		$db->query("DELETE FROM `membergroup` WHERE `id`=$this->id");
	}

	public function update(Database $db) {
		$t_name = mysql_escape_string("$this->name");
		$t_discount = mysql_escape_string("$this->discount");
		$t_minpoint = mysql_escape_string("$this->minpoint");
		$t_authority = mysql_escape_string("$this->authority");
		$SQLINSERT = "INSERT INTO `membergroup` (`name`, `discount`, `minpoint`, `authority`) VALUES ('$t_name', $t_discount, $t_minpoint, $t_authority)";
		$SQLUPDATE = "UPDATE `membergroup` SET `id` = $this->id, `name` = '$t_name', `discount` = $t_discount, `minpoint` = $t_minpoint, `authority` = $t_authority WHERE `id` = $this->id";
		if ($this->id <= 0) $db->query($SQLINSERT); else $db->query($SQLUPDATE);
	}

	public static function fetchRecords($db, $condition = "", $recPerPage = 10, $page = 0) {
		$arr = Array();
		$result = $db->fetch('membergroup', $condition, $recPerPage, $page * $recPerPage);
		while ($row = $db->fetchRow($result)) {
			$tmp = new membergroup();
			$tmp->fromRow($row);
			$arr[] = $tmp;
		}
		return $arr;
	}

}

class Member {

	var $id = -1, $username = "", $password = "", $gender = 0, $birthday = "", $cardno = "", $accountremain = 0, $expire = "", $status = 0, $groupid = 0, $point = 0;

	public function fromRow(Array $row) {
		$this->id = !isset($row['id']) ? $this->id : $row['id'];
		$this->username = !isset($row['username']) ? $this->username : $row['username'];
		$this->password = !isset($row['password']) ? $this->password : $row['password'];
		$this->gender = !isset($row['gender']) ? $this->gender : $row['gender'];
		$this->birthday = !isset($row['birthday']) ? $this->birthday : $row['birthday'];
		$this->cardno = !isset($row['cardno']) ? $this->cardno : $row['cardno'];
		$this->accountremain = !isset($row['accountremain']) ? $this->accountremain : $row['accountremain'];
		$this->expire = !isset($row['expire']) ? $this->expire : $row['expire'];
		$this->status = !isset($row['status']) ? $this->status : $row['status'];
		$this->groupid = !isset($row['groupid']) ? $this->groupid : $row['groupid'];
		$this->point = !isset($row['point']) ? $this->point : $row['point'];
	}

	public function delete(Database $db) {
		$db->query("DELETE FROM `member` WHERE `id`=$this->id");
	}

	public function update(Database $db) {
		$t_username = mysql_escape_string("$this->username");
		$t_password = mysql_escape_string("$this->password");
		$t_gender = mysql_escape_string("$this->gender");
		$t_birthday = mysql_escape_string("$this->birthday");
		$t_cardno = mysql_escape_string("$this->cardno");
		$t_accountremain = mysql_escape_string("$this->accountremain");
		$t_expire = mysql_escape_string("$this->expire");
		$t_status = mysql_escape_string("$this->status");
		$t_groupid = mysql_escape_string("$this->groupid");
		$t_point = mysql_escape_string("$this->point");
		$SQLINSERT = "INSERT INTO `member` (`username`, `password`, `gender`, `birthday`, `cardno`, `accountremain`, `expire`, `status`, `groupid`, `point`) VALUES ('$t_username', '$t_password', $t_gender, '$t_birthday', '$t_cardno', $t_accountremain, '$t_expire', $t_status, $t_groupid, $t_point)";
		$SQLUPDATE = "UPDATE `member` SET `id` = $this->id, `username` = '$t_username', `password` = '$t_password', `gender` = $t_gender, `birthday` = '$t_birthday', `cardno` = '$t_cardno', `accountremain` = $t_accountremain, `expire` = '$t_expire', `status` = $t_status, `groupid` = $t_groupid, `point` = $t_point WHERE `id` = $this->id";
		if ($this->id <= 0) $db->query($SQLINSERT); else $db->query($SQLUPDATE);
	}

	public static function fetchRecords($db, $condition = "", $recPerPage = 10, $page = 0) {
		$arr = Array();
		$result = $db->fetch('member', $condition, $recPerPage, $page * $recPerPage);
		while ($row = $db->fetchRow($result)) {
			$tmp = new member();
			$tmp->fromRow($row);
			$arr[] = $tmp;
		}
		return $arr;
	}

}

class Order {

	var $id = -1, $member_id = 0, $placetime = "";

	public function fromRow(Array $row) {
		$this->id = !isset($row['id']) ? $this->id : $row['id'];
		$this->member_id = !isset($row['member_id']) ? $this->member_id : $row['member_id'];
		$this->placetime = !isset($row['placetime']) ? $this->placetime : $row['placetime'];
	}

	public function delete(Database $db) {
		$db->query("DELETE FROM `order` WHERE `id`=$this->id");
	}

	public function update(Database $db) {
		$t_id = mysql_escape_string("$this->id");
		$t_member_id = mysql_escape_string("$this->member_id");
		$t_placetime = mysql_escape_string("$this->placetime");
		$SQLINSERT = "INSERT INTO `order` (`id`, `member_id`, `placetime`) VALUES ($t_id, $t_member_id, '$t_placetime')";
		$SQLUPDATE = "UPDATE `order` SET `id` = $this->id, `id` = $t_id, `member_id` = $t_member_id, `placetime` = '$t_placetime' WHERE `id` = $this->id";
		if ($this->id <= 0) $db->query($SQLINSERT); else $db->query($SQLUPDATE);
	}

	public static function fetchRecords($db, $condition = "", $recPerPage = 10, $page = 0) {
		$arr = Array();
		$result = $db->fetch('order', $condition, $recPerPage, $page * $recPerPage);
		while ($row = $db->fetchRow($result)) {
			$tmp = new order();
			$tmp->fromRow($row);
			$arr[] = $tmp;
		}
		return $arr;
	}

}

class Transaction {

	var $id = -1, $book_id = 0, $qty = 0, $cost = 0, $status = 0, $order_id = 0;

	public function fromRow(Array $row) {
		$this->id = !isset($row['id']) ? $this->id : $row['id'];
		$this->book_id = !isset($row['book_id']) ? $this->book_id : $row['book_id'];
		$this->qty = !isset($row['qty']) ? $this->qty : $row['qty'];
		$this->cost = !isset($row['cost']) ? $this->cost : $row['cost'];
		$this->status = !isset($row['status']) ? $this->status : $row['status'];
		$this->order_id = !isset($row['order_id']) ? $this->order_id : $row['order_id'];
	}

	public function delete(Database $db) {
		$db->query("DELETE FROM `transaction` WHERE `id`=$this->id");
	}

	public function update(Database $db) {
		$t_book_id = mysql_escape_string("$this->book_id");
		$t_qty = mysql_escape_string("$this->qty");
		$t_cost = mysql_escape_string("$this->cost");
		$t_status = mysql_escape_string("$this->status");
		$t_order_id = mysql_escape_string("$this->order_id");
		$SQLINSERT = "INSERT INTO `transaction` (`book_id`, `qty`, `cost`, `status`, `order_id`) VALUES ($t_book_id, $t_qty, $t_cost, $t_status, $t_order_id)";
		$SQLUPDATE = "UPDATE `transaction` SET `id` = $this->id, `book_id` = $t_book_id, `qty` = $t_qty, `cost` = $t_cost, `status` = $t_status, `order_id` = $t_order_id WHERE `id` = $this->id";
		if ($this->id <= 0) $db->query($SQLINSERT); else $db->query($SQLUPDATE);
	}

	public static function fetchRecords($db, $condition = "", $recPerPage = 10, $page = 0) {
		$arr = Array();
		$result = $db->fetch('transaction', $condition, $recPerPage, $page * $recPerPage);
		while ($row = $db->fetchRow($result)) {
			$tmp = new transaction();
			$tmp->fromRow($row);
			$arr[] = $tmp;
		}
		return $arr;
	}

}

class Bookview {

	var $ID = 0, $ISBN = 0;

	public function fromRow(Array $row) {
		$this->ID = !isset($row['ID']) ? $this->ID : $row['ID'];
		$this->ISBN = !isset($row['ISBN']) ? $this->ISBN : $row['ISBN'];
	}

	public function delete(Database $db) {
		$db->query("DELETE FROM `bookview` WHERE `id`=$this->id");
	}

	public function update(Database $db) {
		$t_ID = mysql_escape_string("$this->ID");
		$t_ISBN = mysql_escape_string("$this->ISBN");
		$SQLINSERT = "INSERT INTO `bookview` (`ID`, `ISBN`) VALUES ($t_ID, $t_ISBN)";
		$SQLUPDATE = "UPDATE `bookview` SET `ID` = $this->ID, `ID` = $t_ID, `ISBN` = $t_ISBN WHERE `id` = $this->id";
		if ($this->id <= 0) $db->query($SQLINSERT); else $db->query($SQLUPDATE);
	}

	public static function fetchRecords($db, $condition = "", $recPerPage = 10, $page = 0) {
		$arr = Array();
		$result = $db->fetch('bookview', $condition, $recPerPage, $page * $recPerPage);
		while ($row = $db->fetchRow($result)) {
			$tmp = new bookview();
			$tmp->fromRow($row);
			$arr[] = $tmp;
		}
		return $arr;
	}

}

class Membergroupview {

	var $ID = 0;

	public function fromRow(Array $row) {
		$this->ID = !isset($row['ID']) ? $this->ID : $row['ID'];
	}

	public function delete(Database $db) {
		$db->query("DELETE FROM `membergroupview` WHERE `id`=$this->id");
	}

	public function update(Database $db) {
		$t_ID = mysql_escape_string("$this->ID");
		$SQLINSERT = "INSERT INTO `membergroupview` (`ID`) VALUES ($t_ID)";
		$SQLUPDATE = "UPDATE `membergroupview` SET `ID` = $this->ID, `ID` = $t_ID WHERE `id` = $this->id";
		if ($this->id <= 0) $db->query($SQLINSERT); else $db->query($SQLUPDATE);
	}

	public static function fetchRecords($db, $condition = "", $recPerPage = 10, $page = 0) {
		$arr = Array();
		$result = $db->fetch('membergroupview', $condition, $recPerPage, $page * $recPerPage);
		while ($row = $db->fetchRow($result)) {
			$tmp = new membergroupview();
			$tmp->fromRow($row);
			$arr[] = $tmp;
		}
		return $arr;
	}

}

class Memberview {

	var $ID = 0;

	public function fromRow(Array $row) {
		$this->ID = !isset($row['ID']) ? $this->ID : $row['ID'];
	}

	public function delete(Database $db) {
		$db->query("DELETE FROM `memberview` WHERE `id`=$this->id");
	}

	public function update(Database $db) {
		$t_ID = mysql_escape_string("$this->ID");
		$SQLINSERT = "INSERT INTO `memberview` (`ID`) VALUES ($t_ID)";
		$SQLUPDATE = "UPDATE `memberview` SET `ID` = $this->ID, `ID` = $t_ID WHERE `id` = $this->id";
		if ($this->id <= 0) $db->query($SQLINSERT); else $db->query($SQLUPDATE);
	}

	public static function fetchRecords($db, $condition = "", $recPerPage = 10, $page = 0) {
		$arr = Array();
		$result = $db->fetch('memberview', $condition, $recPerPage, $page * $recPerPage);
		while ($row = $db->fetchRow($result)) {
			$tmp = new memberview();
			$tmp->fromRow($row);
			$arr[] = $tmp;
		}
		return $arr;
	}

}

class Transactionview {

	var $ID = 0;

	public function fromRow(Array $row) {
		$this->ID = !isset($row['ID']) ? $this->ID : $row['ID'];
	}

	public function delete(Database $db) {
		$db->query("DELETE FROM `transactionview` WHERE `id`=$this->id");
	}

	public function update(Database $db) {
		$t_ID = mysql_escape_string("$this->ID");
		$SQLINSERT = "INSERT INTO `transactionview` (`ID`) VALUES ($t_ID)";
		$SQLUPDATE = "UPDATE `transactionview` SET `ID` = $this->ID, `ID` = $t_ID WHERE `id` = $this->id";
		if ($this->id <= 0) $db->query($SQLINSERT); else $db->query($SQLUPDATE);
	}

	public static function fetchRecords($db, $condition = "", $recPerPage = 10, $page = 0) {
		$arr = Array();
		$result = $db->fetch('transactionview', $condition, $recPerPage, $page * $recPerPage);
		while ($row = $db->fetchRow($result)) {
			$tmp = new transactionview();
			$tmp->fromRow($row);
			$arr[] = $tmp;
		}
		return $arr;
	}

}

?>