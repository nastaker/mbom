IF EXISTS ( SELECT 1 FROM SYSOBJECTS WHERE TYPE = 'U' AND NAME = 'TN_80_APP_0040_MBOM_VER' )
BEGIN
	DROP TABLE TN_80_APP_0040_MBOM_VER
END
-- ��¼MBOM�汾��ʵ����ÿ�����ݶ���һ�����κ�
CREATE TABLE [dbo].[TN_80_APP_0040_MBOM_VER](
	[CN_ID] [INT] IDENTITY(1,1) NOT NULL,					--������ID
	[CN_GUID] [uniqueidentifier] NOT NULL,					--�汾����GUID
	[CN_GUID_PBOM] [uniqueidentifier] not null,				--��ӦPBOM����GUID������Ϊ��
	[CN_STATUS] [varchar](1) NOT NULL,						--��Ч��ʶ��Y����Ч��N��ʧЧ����
	[CN_VER] [varchar](10) NOT NULL,						--�汾��
	[CN_NAME] [varchar](48) NOT NULL,						--��������
	[CN_CODE] [varchar](24) NOT NULL,						--���ϴ���
	[CN_ITEM_CODE] [varchar](18) NOT NULL,					--���ϱ���
	[CN_DESC] [varchar](128),								--�汾��ע˵��
	[CN_IS_TOERP] [SMALLINT] NOT NULL DEFAULT(0),			--�汾�Ƿ��Ѿ�����ERP
	[CN_DT_TOERP] [DATETIME] NOT NULL DEFAULT('2100-01-01'),--�汾����ERPʱ��
	[CN_DT_CREATE] [datetime] NOT NULL DEFAULT(GETDATE()),	--����ʱ��
	[CN_DT_EFFECTIVE] [date] NOT NULL DEFAULT('2000-01-01'),--�汾��Ч����
	[CN_DT_EXPIRY] [date] NOT NULL DEFAULT('2100-01-01'),	--�汾ʧЧ����
	[CN_CREATE_BY] [int] NOT NULL,							--������ID
	[CN_CREATE_NAME] [nvarchar](32),						--����������
	[CN_CREATE_LOGIN] [nvarchar](32)						--�����˵�¼��
	
)
GO
IF EXISTS ( SELECT 1 FROM SYSOBJECTS WHERE TYPE = 'U' AND NAME = 'TN_80_APP_0040_MBOM_VER_HLINK' )
BEGIN
	DROP TABLE TN_80_APP_0040_MBOM_VER_HLINK
END
-- ��¼MBOM�汾��Ӧ��BOM���ݣ�������MBOM�汾ʱ��ȷ����������ȡ����
CREATE TABLE [dbo].[TN_80_APP_0040_MBOM_VER_HLINK](
	[CN_ID] [INT] IDENTITY(1,1) NOT NULL,					--������ID
	[CN_GUID_VER] [uniqueidentifier] NOT NULL,				--MBOM�汾GUID
	[CN_GUID] [uniqueidentifier] NOT NULL,					--���ӹ�ϵGUID
	[CN_STATUS] [VARCHAR](10) NOT NULL DEFAULT('Y'),		--����״̬��Y�������ã�N��δ���ã�
	[CN_TYPE] [varchar](10) NOT NULL DEFAULT(''),			--�������ͣ�V�������C���ϼ���U���Զ������ã�
	[CN_DT_CREATE] [datetime] NOT NULL DEFAULT(GETDATE()),	--����ʱ��
	[CN_DESC] [varchar](128),								--�汾��ע˵��
	[CN_CREATE_BY] [int] NOT NULL,							--������ID
	[CN_CREATE_NAME] [nvarchar](32),						--����������
	[CN_CREATE_LOGIN] [nvarchar](32)						--�����˵�¼��
)
GO
IF EXISTS ( SELECT 1 FROM SYSOBJECTS WHERE TYPE = 'U' AND NAME = 'TN_80_APP_0040_MBOM_HLINK' )
BEGIN
	DROP TABLE TN_80_APP_0040_MBOM_HLINK
END
CREATE TABLE [dbo].[TN_80_APP_0040_MBOM_HLINK](
	[CN_ID] [int] IDENTITY(1,1) NOT NULL,					--������ID
	[CN_GUID] [uniqueidentifier] NOT NULL,					--���ӹ�ϵGUID����PBOM�̳л���MBOM�����ɡ�[��������CODE]��[����CODE]��[����]��[������ϵGUID]�ĸ��ֶ���ͬʱ����GUIDΨһ
	[CN_GUID_LINK] [uniqueidentifier],						--������ϵGUID������MBOMHLINK�������������й���ʱ�����ֶμ�¼[������¼]��[���ӹ�ϵGUID]
	[CN_GUID_EF] [uniqueidentifier],						--��Ч�汾GUID������������¼ʱ��MBOM_VER.CN_GUID
	[CN_GUID_EX] [uniqueidentifier],						--ʧЧ�汾GUID��ʧЧ������¼ʱ��MBOM_VER.CN_GUID
	[CN_ITEMCODE_PARENT] [varchar](24) NOT NULL,			--��������CODE
	[CN_ITEMCODE] [varchar](24) NOT NULL,					--����CODE
	[CN_QUANTITY] [float] NOT NULL,							--����
	[CN_DISPLAYNAME] [varchar](80) NOT NULL,				--��ʾ����
	[CN_ORDER] [int] NOT NULL,								--����
	[CN_UNIT] [nvarchar](10),								--��λ
	[CN_ISASSEMBLY] [bit],									--�Ƿ�ӵ���Ӽ���ͬʱ��Ϊǰ���Ƿ���ʾ�Ӽ����ж�
	[CN_ISBORROW] [bit],									--�Ƿ����
	[CN_ISMBOM] [bit] DEFAULT(1) NOT NULL,					--�Ƿ�ΪMBOM����������
	[CN_DT_CREATE] [datetime] NOT NULL DEFAULT(GETDATE()),	--�������ڣ�Ĭ��ֵGETDATE()
	[CN_DT_EFFECTIVE_PBOM] [date] NOT NULL DEFAULT('2000-01-01'),--��Ч���ڣ�����PBOM����Ч����
	[CN_DT_EXPIRY_PBOM] [date] NOT NULL DEFAULT('2100-01-01'),	--ʧЧ���ڣ�����PBOM��ʧЧ����
	[CN_DT_EFFECTIVE] [date] NOT NULL DEFAULT('2000-01-01'),--��Ч���ڣ�����MBOM_VER����Ч����
	[CN_DT_EXPIRY] [date] NOT NULL DEFAULT('2100-01-01'),	--ʧЧ���ڣ�����MBOM_VER��ʧЧ����
	[CN_DT_TOERP] [datetime] NOT NULL DEFAULT('2100-01-01'),--ERP��������
	[CN_IS_TOERP] [smallint] NOT NULL DEFAULT(0),			--����״̬
	[CN_DESC] [nvarchar](128),								--��ע��Ϣ
	[CN_CREATE_BY] [int] NOT NULL,							--������ID
	[CN_CREATE_NAME] [nvarchar](32),						--����������
	[CN_CREATE_LOGIN] [nvarchar](32)						--�����˵�¼��
)

IF EXISTS ( SELECT 1 FROM SYSOBJECTS WHERE TYPE = 'U' AND NAME = 'TN_80_APP_0040_MBOM_RELEASE' )
BEGIN
	DROP TABLE TN_80_APP_0040_MBOM_RELEASE
END
GO
CREATE TABLE TN_80_APP_0040_MBOM_RELEASE
(
	CN_ID INT IDENTITY(1,1) PRIMARY KEY,		--������ID
	CN_BATCHID INT NOT NULL,					--����ID	
	CN_SIGN_SYS VARCHAR(10) NOT NULL,			--��ʶ
	CN_ITEMCODE_PARENT VARCHAR(32) NOT NULL,	--�������Ϻ�
	CN_PRODUCT_ITEMCODE VARCHAR(32) NOT NULL,	--��Ʒ���Ϻ�
	CN_IS_TOERP SMALLINT NOT NULL,				--�Ƿ��ѷ���
	CN_DT_TOERP DATETIME NOT NULL,				--��������
	CN_DESC VARCHAR(256),						--��ע
	CN_DT_CREATE DATETIME NOT NULL,				--�������ڣ�Ĭ��GETDATE()
	CN_CREATE_BY INT NOT NULL,					--������ID
	CN_CREATE_NAME VARCHAR(32) NOT NULL,		--����������
	CN_CREATE_LOGIN VARCHAR(32) NOT NULL		--�����˵�¼��
)
ALTER TABLE TN_80_APP_0040_MBOM_RELEASE
ADD CONSTRAINT DF_TN_80_APP_0040_MBOM_RELEASE_CN_SIGN_SYS  DEFAULT('') FOR CN_SIGN_SYS
ALTER TABLE TN_80_APP_0040_MBOM_RELEASE
ADD CONSTRAINT DF_TN_80_APP_0040_MBOM_RELEASE_CN_DT_TOERP  DEFAULT('2100-01-01') FOR CN_DT_TOERP
ALTER TABLE TN_80_APP_0040_MBOM_RELEASE
ADD CONSTRAINT DF_TN_80_APP_0040_MBOM_RELEASE_CN_IS_TOERP DEFAULT(0) FOR CN_IS_TOERP
ALTER TABLE TN_80_APP_0040_MBOM_RELEASE
ADD CONSTRAINT DF_TN_80_APP_0040_MBOM_RELEASE_CN_DT_CREATE DEFAULT(GETDATE()) FOR CN_DT_CREATE