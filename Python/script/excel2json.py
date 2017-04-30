#coding:utf-8

'''
@ copyRight WonPeace  

Maintaince Logs: 
2015-07-31 	Initial version
2015-08-02	1.0版本 添加两种导入方式
2015-08-03	修复一个错误
2017-04-30	取值类型进行封装、对sheet转Json进行了封装

'''

import xlrd
import sys
import os

reload(sys)
sys.setdefaultencoding('utf8')

#ignoreRows 为忽略的数组:比如： [2]
def excelToJson(fileName, outputFile,ignoreRows):

	excelFile = xlrd.open_workbook(fileName) #表格
	sheet = excelFile.sheet_by_index(0)

	return sheetToJson(sheet, outputFile, ignoreRows, False);

#将ID提取到外面，ignoreRows 为忽略的数组:比如： [2]
def excelToJsonById(fileName, outputFile,ignoreRows):

	excelFile = xlrd.open_workbook(fileName) #表格
	sheet = excelFile.sheet_by_index(0)

	return sheetToJson(sheet, outputFile, ignoreRows, True);

def printSheetName(filename):

	excelFile = xlrd.open_workbook(filename) #表格
	
	print (str(excelFile.nsheets) +  "-----")
	for sheet_index in range(1, excelFile.nsheets): 
		print "the sheet  " + excelFile.sheet_by_index(sheet_index).name

	for sheet in excelFile.sheets():
		print "the sheet name is " + sheet.name

'''
跟据Sheet的值属性返回 值属性数组 
0:int  1: 字符串 2:float
默认从0判断起，当为字符串就全确定了值类型
'''
def getListTypeBySheet(sheet, ignoreRows):
	#取  总行数 总列
	sumByRow = sheet.nrows;
	sumByCol = sheet.ncols;

	valueTypeList = []

	for col in range(sumByCol):
		valueTypeList.append(0)
		#开始读列
		for row in range(sumByRow):
			#第1行为key 第2行一般为描述 读第二行之后的
			if row == 0:
				continue
			#忽略行
			if(ignoreRows.count(row + 1) > 0):
				continue

			#默认为值，如果有字符串则为字符串
			value = sheet.cell(row,col).value
			# 类型 0 empty,1 string, 2 number, 3 date, 4 boolean, 5 error
			ctype = sheet.cell(row,col).ctype
			#print(ctype)

			if ctype == 2:
				if(round(float(value)) != float(value)): 
					valueTypeList[col] = 2
			elif ctype == 1:
				valueTypeList[col] = 1
				#print "第" + str(col) + "列是字符串 值为：" + str(value) + "  第" + str(row) + " 行出现" 
				break

	return valueTypeList

'''
把一个Sheet 转换成Json
outputFile - 最终Json
ignoreRows - 忽略表里的哪一行
isDict - 是否字典形式
'''
def sheetToJson(sheet, outputFile, ignoreRows, isDict):

	#取  总行数 总列
	sumByRow = sheet.nrows;
	sumByCol = sheet.ncols;

	#所有的Keys (包括格式)
	keyList = []
	for index in range(sumByCol):
		rangeContentOfRow = sheet.row_values(0)
		keyList.append('\t\t\"' + rangeContentOfRow[index].strip().lstrip() + '\":')

	#取列的属性
	valueTypeList = getListTypeBySheet(sheet, ignoreRows)

	#------写入头
	outputFile.write('[\n')

	#循环写入
	for row in range(sumByRow):
		#key所在行
		if(row == 0):
			continue
		#忽略行
		if(ignoreRows.count(row + 1) > 0):
			continue

		#行开头
		outputFile.write('\t{\n')

		#取每一列  写入 key : value 
		for col in range(sumByCol):
			value = sheet.row_values(int(row))[int(col)]

			#提出ID在最前，适用于 dictionary
			if col == 0 and isDict:
				#行开头
				outputFile.write('\t\"' + str(int(value)) + '\":{\n')
				continue

			if valueTypeList[col] == 0 or valueTypeList[col] == 2: 
				if len(str(value)) < 1: value = 0;
				if valueTypeList[col] == 0:
					outputFile.write(keyList[col] + str(int(value)))
				else:
					outputFile.write(keyList[col] + str(value))
			else:
				outputFile.write(keyList[col] + '\"' + str(value) + '\"')

			#若非最后一列，则加入逗号
			if col != sumByCol:
				outputFile.write(',\n')
			#最后一个，加入结尾标记
			else:
				outputFile.write('\n')

		#行结尾
		if row != sumByRow:
			outputFile.write('\t},\n')
		else:
			outputFile.write('\t}\n')

	#------写入尾
	outputFile.write(']')
	(filepath,filename)=os.path.split(outputFile.name)
	print  " -export succeed!!!!- " + str(filename)
	return outputFile














