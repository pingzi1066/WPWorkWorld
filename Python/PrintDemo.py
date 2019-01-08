#coding:utf-8

'''
测试用

'''
#shutil : 复制文件、删除非空目录
import os,platform,hashlib,shutil

# 工具脚本所在目录
import script.excel2json
convert = script.excel2json

rootPath = os.path.split(os.path.realpath(__file__))[0]


mainPath = rootPath + "/excel2json"

excelPath = mainPath + "/table/Boss.xlsx" #表格位置

convert.printSheetName(excelPath)

