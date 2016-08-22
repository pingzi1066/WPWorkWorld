#coding:utf-8

'''
@ copyRight WonPeace  

Maintaince Logs: 
2015-08-01 Initial version


'''
import sys,os

rootPath = os.path.split(os.path.realpath(__file__))[0]

excelPath = rootPath + "/Assets/Doc/Excel/" #表格位置
exportPath = rootPath + "/Assets/Resources/json/" #导出的所有的json目录
recordPath = rootPath + "/record/" #文件导出记录
exportNewPath = rootPath + "/newJson/" #当前导出已经改变过的json目录

# 工具脚本所在目录
sys.path.append("../Python/script/")
import convertExcel

convertExcel.convert(excelPath,exportPath,recordPath,exportNewPath)