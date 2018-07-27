# 中文转拼音组件

暂不支持多音字智能识别，存在多音字则返回多种情况的组合

### 安装方式


To install SchoolPal.Toolkit.Pinyins from the Package Manager Console, run the following command:
```
Install-Package NetCorePal.Toolkit.Pinyins
```



### 中文转拼音首字母

```
var v = PinyinConvert.ToPinyinInitials("行行好吧");

//上面代码返回数组 ["HHHB","XHHB","HXHB","XXHB"]
//"行"有三个读音 HANG HENG XING 但首字母去重后有两个：H X，共四种组合

```

### 中文转拼音
```
var v = PinyinConvert.ToPinyins("行行好吧");

//返回数组：
["XINGXINGHAOBA"，"XINGHENGHAOBA"，"XINGHANGHAOBA"，"HENGXINGHAOBA"，"HENGHENGHAOBA"，"HENGHANGHAOBA"，"HANGXINGHAOBA"，"HANGHENGHAOBA"，"HANGHANGHAOBA"]


```

### 中文转拼音首字母和拼音拼接字符串

```
var v = PinyinConvert.ToPinyinSearchFomat("行行好吧");
//分隔符默认为";"
//上面代码返回字符串："HHHB;XHHB;HXHB;XXHB;HANGHANGHAOBA;HENGHANGHAOBA;XINGHANGHAOBA;HANGHENGHAOBA;HENGHENGHAOBA;XINGHENGHAOBA;HANGXINGHAOBA;HENGXINGHAOBA;XINGXINGHAOBA"
```

# 中文名称转拼音

### 安装
Install-Package NetCorePal.Toolkit.Pinyins.ChineseName

### 中文姓名转拼音
var result = ChineseNamePinyinConvert.GetChineseNamePinYin("单雄信");

### 返回结果：
shanxiongxin

### 说明：
同时支持net45和netcore，支持百家姓多音字