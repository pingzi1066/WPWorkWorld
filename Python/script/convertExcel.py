#coding:utf-8

'''
@ copyRight WonPeace  

Maintaince Logs: 
2015-08-01 	Initial version
2017-04-30	封装判断，去掉相同的代码
			加入了Sheet全部转换的代码

'''
import sys,os,excel2json,kmTools, xlrd

reload(sys)
sys.setdefaultencoding('utf8')


# rootPath = os.path.split(os.path.realpath(__file__))[0] + "/excel2json"

# excelPath = rootPath + "/table/" #表格位置
# exportPath = rootPath + "/json/" #导出的所有的json目录
# recordPath = rootPath + "/record/" #文件导出记录
# exportNewPath = rootPath + "/new/" #当前导出已经改变过的json目录

def convert(excelPath, exportPath, recordPath, exportNewPath):

	inputStr = raw_input("export all excel please enter input: yes\n")

	#清空新导出的new文件夹
	kmTools.del_dir(exportNewPath)
	kmTools.mkdir(exportNewPath)

	#先创建相关的目录
	kmTools.mkdir(recordPath)
	kmTools.mkdir(exportPath)

	#从根目录获得所有的分目录、文件名。root 为地址，dirs 为地址下的文件夹 、 files为地址下的文件
	for root, dirs, files in os.walk(excelPath):
		for f in files:
			#取文件名 + 后缀
			fname, fextension = os.path.splitext(f)
			#是表格文件
			if(fextension == ".xlsx" or fextension == ".csv" or fextension == ".xls"):
				#检查记录中是否已经是导出过最新的文件
				curMd5 = kmTools.getFileMd5(excelPath + f)

				recordFile = kmTools.openFile(recordPath + fname + ".txt",'r')
				preMd5 = recordFile.read()
				recordFile.close()

				# 写入条件：用户输入重导入 或者 判断时文件有所改变
				if curMd5 != preMd5 or str(inputStr) == 'yes':
					outputFilePath = exportPath + "Static" + fname[0:1].upper() + fname[1:] + ".json"
					outputFile = open(outputFilePath,'w')

					excel2json.excelToJsonById(excelPath + f,outputFile, [2])

					#复制到新文件夹
					kmTools.copy_file(outputFilePath,exportNewPath)

					recordFile = kmTools.openFile(recordPath + fname + ".txt",'w')
					recordFile.write(curMd5)
					recordFile.close()
				else :
					print  " -file is not change!!!!!- " + f

'''
转换一个Excel的所有的Sheet
每个单独的表导出后，
第一个表的名字为默认名字
后面的名字为原来名字 + Sheet名字（Sheet首字母大写）
其它的逻辑保持一样
'''
def convertAllSheet(excelPath, exportPath, recordPath, exportNewPath):
	inputStr = raw_input("export all excel please enter input: yes\n")

	#清空新导出的new文件夹
	kmTools.del_dir(exportNewPath)
	kmTools.mkdir(exportNewPath)

	#先创建相关的目录
	kmTools.mkdir(recordPath)
	kmTools.mkdir(exportPath)

	#从根目录获得所有的分目录、文件名。root 为地址，dirs 为地址下的文件夹 、 files为地址下的文件
	for root, dirs, files in os.walk(excelPath):
		for f in files:
			#取文件名 + 后缀
			fname, fextension = os.path.splitext(f)
			#是表格文件
			if(fextension == ".xlsx" or fextension == ".csv" or fextension == ".xls"):

				excelName = excelPath + f
				#检查记录中是否已经是导出过最新的文件
				curMd5 = kmTools.getFileMd5(excelName)

				recordFile = kmTools.openFile(recordPath + fname + ".txt",'r')
				preMd5 = recordFile.read()
				recordFile.close()

				# 写入条件：用户输入重导入 或者 判断时文件有所改变
				if curMd5 != preMd5 or str(inputStr) == 'yes':
					#导出第一个表格
					outputFilePath = exportPath + "Static" 
					outputFilePath += fname[0:1].upper() + fname[1:]
					outputFilePath += ".json"

					outputFile = open(outputFilePath,'w')

					excel2json.excelToJsonById(excelName, outputFile, [2])
					kmTools.copy_file(outputFilePath,exportNewPath)

					#导出其它表格
					excelFile = xlrd.open_workbook(excelName)
					for sheet_index in range(1, excelFile.nsheets): 

						sheet = excelFile.sheet_by_index(sheet_index)
						sheetName = sheet.name

						outputFilePath = exportPath + "Static" 
						outputFilePath += fname[0:1].upper() + fname[1:] 
						outputFilePath += sheetName[0:1].upper() + sheetName[1:]
						outputFilePath += ".json"
						outputFile = open(outputFilePath,'w')

						excel2json.sheetToJson(sheet, outputFile, [2], True)

						#复制到新文件夹
						kmTools.copy_file(outputFilePath,exportNewPath)

					recordFile = kmTools.openFile(recordPath + fname + ".txt",'w')
					recordFile.write(curMd5)
					recordFile.close()
				else :
					print  " -file is not change!!!!!- " + f
