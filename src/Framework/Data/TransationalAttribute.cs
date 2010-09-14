using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;



using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;


namespace InfoControl.Data
{
	/// <summary>
	/// Este atributo é herdado de ContextAttribute e IContributeObjectSink
	/// <para>Atributo deve ser usado em uma classe filha de ContextBoundObject para funcionar</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class TransactionAttribute: ContextAttribute, IContributeObjectSink
	{
		private System.Data.IsolationLevel _isolationLevel;

		/// <summary>
		/// Este atributo é herdado de ContextAttribute e IContributeObjectSink
		/// <para>Atributo deve ser usado em uma classe filha de ContextBoundObject para funcionar</para>
		/// </summary>
		/// <param name="isolationLevel">Nivel de isolação da transação</param>
		public TransactionAttribute(System.Data.IsolationLevel isolationLevel): base("Transaction")
		{
			_isolationLevel = isolationLevel;			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctorMsg"></param>
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			ctorMsg.ContextProperties.Add(this);
		}

		/// <summary>
		/// Este metodo é chamado em runtime para verifica 
		/// se o contexto passado é um contexto válido
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="ctorMsg"></param>
		/// <returns></returns>
		public override bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			return false;
		}


		/// <summary>
		/// Função que realiza a Intercepção do metodo que tem este atributo
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="nextSink"></param>
		/// <returns></returns>
		public IMessageSink GetObjectSink ( System.MarshalByRefObject obj , IMessageSink nextSink )
		{
			return(new TransactionMessageSink(nextSink));
		}
	}






	/// <summary>
	/// Esta classe irá fazer o INTERCEPTOR onde irá disparar o metodo e pegará seu resultado
	/// caso tenha Exception foi pq algo deu errado então Rollback
	/// </summary>
	public class TransactionMessageSink: IMessageSink
	{
		private IMessageSink _next;

		/// <summary>
		/// Guarda a próxima mensagem pois apartir dela irá realizar a sequencia de funções
		/// </summary>
		/// <param name="next"></param>
		public TransactionMessageSink(IMessageSink next)
		{
			_next = next;
		}


		/// <summary>
		/// Apenas para implementar a interface
		/// </summary>
		public IMessageSink NextSink
		{
			get{return _next;}
		}

		/// <summary>
		/// Apenas para implementar a interface
		/// </summary>
		public IMessageCtrl AsyncProcessMessage ( IMessage msg , IMessageSink replySink )
		{
			return(_next.AsyncProcessMessage(msg, replySink));
		}
		
/// <summary>
/// 
/// </summary>
/// <param name="msg"></param>
/// <returns></returns>
		public IMessage SyncProcessMessage ( IMessage msg )
		{
			if((msg is IMethodMessage))
				return(_next.SyncProcessMessage(msg));
			return null;
		}

	}




	/// <summary>
	/// Esta classe é responsável por pegar a transação atual
	/// </summary>
	internal class Transaction
	{
		static private string PropertyName
		{
			get{return "Vivina.DataAccess.Transaction";}
		}

		static IDbTransaction CurrentTransaction
		{
			get{return Thread.CurrentContext.GetProperty(PropertyName) as IDbTransaction;}
		}
	}


	/// <summary>
	/// Responsável por identificar o objeto IDbConnection
	/// </summary>
	internal class Connection
	{
		static private string PropertyName
		{
			get{return "Vivina.DataAccess.Connection";}
		}

		static IDbConnection CurrentConnection
		{
			get{return Thread.CurrentContext.GetProperty(PropertyName) as IDbConnection;}
		}
	}


	
}
