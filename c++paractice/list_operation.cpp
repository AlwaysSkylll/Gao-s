#include<stdio.h>
#include<stdlib.h>
#include<iostream>
using namespace std;
#define MAXSIZE 100
typedef struct List;
typedef int ElemenType;
typedef int Position;
struct List
{
	ElemenType Data[MAXSIZE];
	int Lenth;
};

//创建链表L
int MakeEmpty(List *&L)
{
	L=(List *)malloc(sizeof(List));		/*分配空间*/
	int i=0;
	for(i=0;i<MAXSIZE;i++){
		L->Data[i]=-99999;
	}
	L->Lenth=0;
	return 1;
}

//销毁线性表，释放内存
int DestroyList(List *&L)
{
	free(L);
	return 0;
}

//查找L中的P位置的元素
int Find(List *L,Position P)
{
	int Elem;
	Elem=L->Data[P-1];
	return Elem;
}

//链表是否已满
bool IsFull(List *L)
{
	if(L->Lenth==MAXSIZE)
		return true;
	return false;
}

//链表是否为空
bool IsEmpty(List *L)
{
	if(L->Lenth==0)
		return true;
	return false;
}

//顺序插入一个元素到链表L最后
int InsertElem(List *L,ElemenType elem)
{
	if(IsFull(L))
		return 0;
	L->Data[L->Lenth]=elem;
	L->Lenth++;
	return 1;
}
//插入一个元素elem到链表L中指定P位置
int InsertElem(List *L,ElemenType elem,Position P)
{

	if(P>L->Lenth+1)
		return 0;
	int i=L->Lenth;
	for(;i>=P;i--)
	{
		L->Data[i]=L->Data[i-1];
	}
	L->Data[i]=elem;
	L->Lenth++;
	return 1;
}

//打印链表内容
int PrintList(List *L)
{
	if(IsEmpty(L))
		return 0;
	for(int i=0;i<L->Lenth;i++)
	{
		cout<<L->Data[i]<<" ";
	}
	return 1;
}

//删除P位置所在的元素
int DeleteElem(List *L,Position P)
{
	if(P>L->Lenth)
		return 0;
	if(IsEmpty(L))
		return 0;
	for(int i=P-1;i<L->Lenth;i++)
	{
		L->Data[i]=L->Data[i+1];
	}
	L->Lenth--;
	return 1;
}

//交换两个位置P，P0的元素
int SwapElement(List *L,Position A,Position B)
{
	if(IsEmpty(L))
		return 0;
	if((A>L->Lenth)||(B>L->Lenth))
		return 0;
	if(A==B)
		return 2;
	ElemenType temp;
	temp=L->Data[A-1];
	L->Data[A-1]=L->Data[B-1];
	L->Data[B-1]=temp;
	return 1;
}

//修改指定P位置的元素内容
int ModifyElement(List *L,Position P,ElemenType elem)
{
	if((P>L->Lenth)||IsEmpty(L))
		return 0;
	L->Data[P-1]=elem;
	return 1;
}

//操作平台
int OperateGround()
{
	List *L;
	Position P;
	Position P0;
	ElemenType Elem;
	int Option;
	cout<<"============================List Operate test============================"<<endl;
	cout<<"			1 Create and initial a List (*First Step)"<<endl;
	cout<<"			2 Insert an elemnt at the end of the list"<<endl;
	cout<<"			3 Insert an elemnt at an effective position"<<endl;
	cout<<"			4 Find what the element is with a known-position"<<endl;
	cout<<"			5 Delete a element at the position you select"<<endl;
	cout<<"			6 Swap the position of two element"<<endl;
	cout<<"			7 Modify the element of some position"<<endl;
	cout<<"			0 Print the List with the order"<<endl;
	cout<<"			-1 Exit"<<endl;
	cout<<"			Please choose yout option above"<<endl;
	cout<<"==================================End=================================="<<endl;
	
	while (1)
	{
		cin>>Option;
		switch (Option) 
		{
		default:
			break;
		case -1:
			DestroyList(L);
			return 0;
		case 0:
			if(!PrintList(L))
				cout<<"Sorry,there must be some problems.";
			break;
		case 1:
			if(MakeEmpty(L))
				cout<<"Congratulations.The list is Created.";
			else
				cout<<"Sorry,there must be some problems.";
			break;
		case 2:
			cout<<"Input the element:";
			cin>>Elem;
			if(InsertElem(L,Elem))
				cout<<"Insert successfully.";
			else
				cout<<"Failed.Sorry,the list is full.";
			break;
		case 3:
			cout<<"Input the element and the position(such as:5 1):";
			cin>>Elem>>P;
			if(IsFull(L))
				cout<<"Failed.Sorry,the list is full.";
			else if(InsertElem(L,Elem,P))
				cout<<"Insert successfully.";
			else
				cout<<"Sorry,there must be some problems.";
			break;
		case 4:
			cout<<"Input the position:";
			cin>>P;
			if(P>L->Lenth)
				cout<<"Sorry,there must be some problems.";
			else
				{
					Elem=Find(L,P);
					cout<<"The element is "<<Elem<<".";
				}				
			break;
		case 5:
			cout<<"Input the position:";
			cin>>P;
			if(DeleteElem(L,P))
				cout<<"The element is deleted.";	
			else
				cout<<"Sorry,there must be some problems.";
			break;
		case 6:
			cout<<"Input positions you want to swap(such as 1 2):";
			cin>>P>>P0;
			if(SwapElement(L,P,P0))
				cout<<"Swap successfully.";
			else
				cout<<"Sorry,there must be some problems.";
			break;
		case 7:
			cout<<"Input the position and the element you'd like to modify to(such as 5 12):";
			cin>>P>>Elem;
			if(ModifyElement(L,P,Elem))
				cout<<"Modify already.";
			else
				cout<<"Sorry,there must be some problems.";
			break;
		}
		cout<<"What do you want next"<<endl;
	}
	return 0;
}
//主函数入口
int main()
{
	if(!OperateGround())
		return 0;
}
