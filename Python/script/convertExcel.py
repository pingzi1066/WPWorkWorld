#coding:utf-8

'''
@ copyRight WonPeace  

Maintaince Logs: 
2015-08-01 Initial version


'''
import sys,os,excel2json,kmTools

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

				if str(inputStr) == 'yes':
					outputFilePath = exportPath + "Static" + fname[0:1].upper() + fname[1:] + ".json"
					outputFile = open(outputFile,'w')
					excel2json.excelToJsonById(excelPath + f,outputFile, [2])
					#复制到新文件夹
					kmTools.copy_file(outputFilePath,exportNewPath)

					recordFile = kmTools.openFile(recordPath + fname + ".txt",'w')
					recordFile.write(curMd5)
					recordFile.close()
					continue

				recordFile = kmTools.openFile(recordPath + fname + ".txt",'r')
				preMd5 = recordFile.read()
				recordFile.close()

				if curMd5 == preMd5:
					print  " -file is not change!!!!!- " + f
				else :
					outputFilePath = exportPath + "Static" + fname[0:1].upper() + fname[1:] + ".json"
					outputFile = open(outputFilePath,'w')

					excel2json.excelToJsonById(excelPath + f,outputFile, [2])

					#复制到新文件夹
					kmTools.copy_file(outputFilePath,exportNewPath)

					recordFile = kmTools.openFile(recordPath + fname + ".txt",'w')
					recordFile.write(curMd5)
					recordFile.close()
