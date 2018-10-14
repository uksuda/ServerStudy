#ifndef __SERVER_UTILS_H__
#define __SERVER_UTILS_H__

#define SOCKET_OPTION_TRUE 1
#define SOCKET_OPTION_FALSE 0

#define SOCKET_TIME_WAIT_FALSE 0

#define BUFFER_SIZE 1024

#define SAFE_DELETE(p) {if (p) { delete (p); (p) = nullptr; }}
#define SAFE_DELETE_ARRAY(p) {if (p) { delete [] (p); (p) = nullptr; }}

#define DECLARE_SINGLETON(classname)							\
		private:												\
			classname() {}										\
			static classname* m_pInstance;						\
		public:													\
			static classname* getInstance();					\
			static void destroyInstance();

#define IMPLEMENT_SINGLETON(classname)							\
		classname* classname::m_pInstance = nullptr;			\
		classname* classname::getInstance(){					\
			if (m_pInstance == nullptr){						\
				m_pInstance = new classname;}					\
			return m_pInstance;}								\
		void classname::destroyInstance(){						\
			SAFE_DELETE(m_pInstance);}


#endif