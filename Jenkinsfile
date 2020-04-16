pipeline
{
    agent {
        node {
            label "nislo-pms"
        }
    }
   stages 
    {
        stage('build') {
            steps {
                echo 'building the  aplication'
            }
        }
        stage('test') {
            steps {
                echo 'testing the  aplication'
            }
        }
        stage('deploy') 
        {
            steps
            {
                echo 'deploying the  aplication'
            }
        }
    }
}
