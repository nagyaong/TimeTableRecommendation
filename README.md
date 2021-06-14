# 소프트웨어 융합대학 시간표 작성 프로그램
------------------
- 기피시간대를 고려해 사용자 맞춤 추천 강의 선택
- 빈 시간 기준 추천 과목 정렬
- 소프트웨어 융합대학 학생들을 대상
- 제작기간 2020.04-2020.06
----------
## 기능
### 수강생 정보 입력 및 맞춤 강의 표기
- 전공, 학년, 학점 입력 -> 사용자 맞춤 강의 목록 표기
- 전공필수, 전공 선택, 교양 과 같은 순으로 정렬 

### 기피 시간대 설정 및 과목 목록 반영
- 요일별/ 시간별 기피 시간대 설정 가능
- 전공필수는 기피 안 됨.

### 강의 선택 및 삭제 & 인강
- 강의 목록에서 선택 시 시간표로 시각화와 이수학점 카운트
- 드롭박스에서 선택한 강의 확인 및 삭제 가능
- 인강 드롭박스에서 인강 확인 및 삭제 가능

### 시간표 저장 및 초기화
- 시간표 초기화 가능
- 이미지로 시간표 저장 가능

### 예외
- 이수학점 초과시 계속해서 강의 담을 수는 있으나, 숫자 색 빨간색으로 변경
- 이미 추가되어있는 시간대에 추가 시도 시 메시지 박스로 알림. 추가 불가
- 