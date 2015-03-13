<?php

class Database
{
	private $link;
	
	public function __construct($dbHost, $dbUser, $dbPass, $dbName)
	{
		
		$link = mysql_connect($dbHost, $dbUser, $dbPass);
		if (!$link)
		{
			throw new Exception('Error while connecting to database: '.mysql_error(), 1);
		}
		if (!mysql_select_db($dbName))
		{
			throw new Exception('Error while selecting the database: '.mysql_error(), 1);
		}
		if (!mysql_query('SET NAMES UTF8'))
		{
			throw new Exception('Error while selecting the default code: '.mysql_error(), 1);
		}
		date_default_timezone_set("UTC+8");
	}
	
	public function __destruct()
	{
		mysql_close();
	}
		
	public function Query($sql)
	{
		$result = mysql_query($sql);
		if (!$result)
		{
			throw new Exception('Error while executing query('.$sql. '): '.mysql_error(), 2);
		}
		return $result;
	}
	
	public function RowsCount($result)
	{
		return mysql_num_rows($result);
	}
	
	public function FetchRow($result)
	{
		return mysql_fetch_array($result);		
	}
	
	private function fetch($table, $condition = "", $maxCount = 100, $offset = 0, $order = "#default#")
	{
		if ($order == "#default#") $order = "ORDER BY ".preg_replace("/s+$/", "", $table)."_date DESC";
		$condition = $condition == "" ? "1=1" : $condition;
		$maxCount = $maxCount > 0 ? $maxCount : 1000;
		$sql = "SELECT * FROM ".table($table)." WHERE $condition $order  LIMIT $maxCount OFFSET $offset";
		try
		{
			$result = $this->Query($sql);
			return $result;
		}
		catch (Exception $e)
		{
			echo $e->getMessage();
		}

	}
		
	public function FetchUsers($condition = "", $maxCount = 1000, $offset = 0)
	{
		$arr = Array();
		$result = $this->fetch('users', $condition, $maxCount, $offset);
		while ($row = $this->FetchRow($result))
		{
			$user = new User();
			$user->FromRow($row);
			$arr["$user->id"] = $user;
		}
		
		return $arr;
	}
	
	public function FetchPosts($condition = "", $maxCount = 1000, $offset = 0)
	{
		$arr = Array();
		
		$TPosts = table('posts');
		$TCategory = table('categories');
		$TUsers = table('users');
		
		if ($condition != "") $condition = "AND ".$condition; 
		$condition .= " AND $TPosts.post_type ='post'";
		
		$result = $this->Query("SELECT $TPosts.*, $TUsers.`user_nicename` as `post_author`, $TCategory.`category_name` as post_category_name 
								FROM $TPosts, $TUsers, $TCategory
								WHERE $TPosts.post_author_id = $TUsers.`user_id` AND ($TPosts.post_category = $TCategory.category_id)
									  AND (($TPosts.`post_status` = 'publish' OR $TPosts.`post_authorized_visitors` LIKE '%,".$_SESSION['login_username'].",%')
									  $condition)
								ORDER BY $TPosts.post_order DESC, $TPosts.post_date DESC
								LIMIT $maxCount OFFSET $offset");
		
		while ($row = $this->FetchRow($result))
		{
			$post = new Post();
			$post->FromRow($row);
			$arr["$post->id"] = $post;
		}
		
		return $arr;
	}
		
	public function FetchLinks($condition = "link_visible = 'Y'", $maxCount = 1000, $offset = 0)
	{
		$arr = Array();
		$result = $this->fetch('links', $condition, $maxCount, $offset, "ORDER BY link_order");
		while ($row = $this->FetchRow($result))
		{
			$tmp = new Link();
			$tmp->FromRow($row);
			$arr["$tmp->id"] = $tmp;
		}
		
		return $arr;
	}
	
	public function FetchComments($condition = "", $maxCount = 1000, $offset = 0)
	{
		$arr = Array();
		$result = $this->fetch('comments', $condition, $maxCount, $offset);
		while ($row = $this->FetchRow($result))
		{
			$tmp = new Comment();
			$tmp->FromRow($row);
			$arr["$tmp->id"] = $tmp;
		}		
		return $arr;
	}
	
	public function FetchPostById($id)
	{
		return $this->FetchPosts(table('posts').".`post_id` = $id");		
	}
	
	public function FetchCommentsById($post_id)
	{
		return $this->FetchComments(table('comments').".`comment_post_id` = $post_id");
	}
	
	public function FetchCategories($id = -1, $cate_parent = -1)
	{
		$arr = Array();
		$condition = '';
		if ($cate_parent >= 0) $condition = "cate_parent = $cateparent";
		if ($id >= 0) $condition = "category_id = $id";
		$result = $this->fetch('categories', $condition, 1000, 0, "");
		while ($row = $this->FetchRow($result))
		{
			$tmp = new Category();
			$tmp->FromRow($row);
			$arr["$tmp->id"] = $tmp;
		}		
		return $arr;
	}
	
	public function FetchBlogOptions($blog_id)
	{
		
		$arr = Array();
		$result = $this->fetch('options', "blog_id = 0$blog_id", 1000, 0, "");
		while ($row = $this->FetchRow($result))
		{
			$arr[$row['option_name']] = $row['option_value'];
		}
		
		return $arr;
	}
	
	public function PushBackBlogOptions($blog_id, $blog)
	{
		
		$sql = "UPDATE ".table('options')." SET ";
		foreach(array_keys($blog) as $opt_name)
		{
			$sql .= $opt_name . "='" . $blog[$opt_name] . "', ";
		}
		$sql .= "blog_id = $blog_id WHERE blog_id = $blog_id";
		
		$this->Query($sql);
		
	}
		
}

?>