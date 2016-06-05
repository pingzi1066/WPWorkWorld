#coding:utf-8

'''
2015-06-14	WP			Initial version.
2015-08-02  WP          add get md5
2016-06-04 加入删除文件夹、复制文件到目录的功能

'''
#shutil : 复制文件、删除非空目录
import os,platform,hashlib,shutil

#取当前文件所在目录
#filePath = os.path.split(os.path.realpath(__file__))[0]

#打开文件方法 ftype 为权限 比如 'r' ，'w'模式打开的文件若存在则先清空，然后重新创建
def openFile(name,ftype):
    fp=None
    sysType = platform.system()
    if os.path.exists(name) == False:
        fp = open(name,'w')
        #fp.close()

    if(sysType =="Windows"):
        try:
            fp=open(name,ftype,encoding='utf-8')
        except:
            fp=open(name,ftype)
    else:
        fp=open(name,ftype)
    
    return fp

#创建目录，返回true为创建成功
def mkdir(path):
    
    path=path.strip()
    
    path=path.rstrip("\\")
    
    isExists=os.path.exists(path)
    
    if not isExists:
        # print path+' OK'
        os.makedirs(path)
        return True
    else:
    # print path+'OK'
        return False

#删除指定目录，返回true为删除成功
def del_dir(path):
    
    path=path.strip()
    
    path=path.rstrip("\\")
    
    isExists=os.path.exists(path)
    if not isExists:
        return False;
    else:
        shutil.rmtree(path)
        return True;

def getFileMd5(fileName):
    fp = openFile(fileName,'rb')
    md5 = hashlib.md5()
    md5.update(fp.read())
    md5 = md5.hexdigest()
    fp.close()
    return md5    

#复制文件到指定目录
def copy_file(file,path):
    isExists=os.path.exists(path)
    if not isExists:
        return False;

    shutil.copy(file, path)
    return True;


