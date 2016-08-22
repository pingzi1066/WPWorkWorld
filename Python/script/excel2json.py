#coding:utf-8

'''
@ copyRight WonPeace  

Maintaince Logs: 
2015-07-31 	Initial version
2015-08-02	1.0版本 添加两种导入方式
2015-08-03	修复一个错误

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

	#------写入头
	outputFile.write('[\n')

	#取  总行数 总列
	sumByRow = sheet.nrows;
	sumByCol = sheet.ncols;

	#所有的Keys (包括格式)
	keyList = []
	for index in range(sumByCol):
		rangeContentOfRow = sheet.row_values(0)
		keyList.append('\t\t\"' + rangeContentOfRow[index].strip().lstrip() + '\":')

	#列的属性：0:int  1: 字符串 2:float
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
				if(round(float(value)) != float(value)): valueTypeList[col] = 2
			elif ctype == 1:
				valueTypeList[col] = 1
				#print "第" + str(col) + "列是字符串 值为：" + str(value) + "  第" + str(row) + " 行出现" 
				break

		#if valueTypeList[col] == 0: print "第" + str(col) + "列是整型"
		#elif valueTypeList[col] == 1: print "第" + str(col) + "列是字符串"
		#else: print "第" + str(col) + "列是浮点型"

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

#将ID提取到外面，ignoreRows 为忽略的数组:比如： [2]
def excelToJsonById(fileName, outputFile,ignoreRows):

	excelFile = xlrd.open_workbook(fileName) #表格
	sheet = excelFile.sheet_by_index(0)

	#------写入头
	outputFile.write('{\n')

	#取  总行数 总列
	sumByRow = sheet.nrows;
	sumByCol = sheet.ncols;

	#所有的Keys (包括格式)
	keyList = []
	for index in range(sumByCol):
		rangeContentOfRow = sheet.row_values(0)
		keyList.append('\t\t\"' + rangeContentOfRow[index].strip().lstrip() + '\":')

	#列的属性：0:int  1: 字符串 2:float
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
				if(round(float(value)) != float(value)): valueTypeList[col] = 2
			elif ctype == 1:
				valueTypeList[col] = 1
				#print "第" + str(col) + "列是字符串 值为：" + str(value) + "  第" + str(row) + " 行出现" 
				break

		#if valueTypeList[col] == 0: print "第" + str(col) + "列是整型"
		#elif valueTypeList[col] == 1: print "第" + str(col) + "列是字符串"
		#else: print "第" + str(col) + "列是浮点型"

	#循环写入
	for row in range(sumByRow):
		#key所在行
		if(row == 0):
			continue
		#忽略行
		if(ignoreRows.count(row + 1) > 0):
			continue

		#取每一列  写入 key : value 
		for col in range(sumByCol):
			value = sheet.row_values(int(row))[int(col)]

			if col == 0:
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
	outputFile.write('}')
	(filepath,filename)=os.path.split(outputFile.name)
	print  " -export succeed!!!!- " + str(filename)
	return outputFile