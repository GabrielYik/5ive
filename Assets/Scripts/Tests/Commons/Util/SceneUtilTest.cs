﻿using NUnit.Framework;

public class SceneUtilTest {

	[Test]
	public void GetLevelName_ValidSceneBuildIndex() {
		Assert.AreEqual(SceneUtil.GetLevelName(0), "Main_Menu");
	}
}
